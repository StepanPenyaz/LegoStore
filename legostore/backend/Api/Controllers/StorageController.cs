using LegoStore.Api.Dtos;
using LegoStore.Domain;
using LegoStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace LegoStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorageController : ControllerBase
{
    private readonly IStorageRepository _repository;
    private readonly IBsxParserService _bsxParser;
    private readonly Func<StoreStorage, IStorageService> _storageServiceFactory;
    private readonly IConfiguration _configuration;

    public StorageController(
        IStorageRepository repository,
        IBsxParserService bsxParser,
        Func<StoreStorage, IStorageService> storageServiceFactory,
        IConfiguration configuration)
    {
        _repository            = repository;
        _bsxParser             = bsxParser;
        _storageServiceFactory = storageServiceFactory;
        _configuration         = configuration;
    }

    /// <summary>Returns the full storage state.</summary>
    [HttpGet]
    public async Task<ActionResult<StorageDto>> GetStorage(CancellationToken ct)
    {
        var storage = await _repository.LoadAsync(ct);
        if (storage is null)
            return NotFound("No storage data found. Please initialise the database first.");

        return Ok(MapToDto(storage));
    }

    /// <summary>
    /// Reads all BSX XML files from the incoming-orders folder, applies the picked lots
    /// to the storage model and persists the updated state to the database.
    /// </summary>
    [HttpPost("update-state")]
    public async Task<IActionResult> UpdateState(CancellationToken ct)
    {
        var folderPath = _configuration["Storage:IncomingOrdersPath"];

        if (string.IsNullOrWhiteSpace(folderPath))
            return BadRequest("Storage:IncomingOrdersPath is not configured. Set it in appsettings.json or via the Storage__IncomingOrdersPath environment variable.");

        if (!Directory.Exists(folderPath))
            return BadRequest($"Incoming orders folder not found: {folderPath}");

        var xmlFiles = Directory.GetFiles(folderPath, "*.xml");
        if (xmlFiles.Length == 0)
            return Ok(new { message = "No XML files found in the incoming orders folder." });

        var storage = await _repository.LoadAsync(ct);
        if (storage is null)
            return NotFound("No storage data found. Please initialise the database first.");

        var allLots = new List<PickedLot>();
        var errors  = new List<string>();

        foreach (var file in xmlFiles)
        {
            try
            {
                allLots.AddRange(_bsxParser.Parse(file));
            }
            catch (Exception ex)
            {
                errors.Add($"{Path.GetFileName(file)}: {ex.Message}");
            }
        }

        var service = _storageServiceFactory(storage);
        service.ApplyPickedLots(allLots);

        await _repository.SaveAsync(storage, ct);

        return Ok(new
        {
            message      = $"Processed {xmlFiles.Length} file(s), applied {allLots.Count} lot(s).",
            filesCount   = xmlFiles.Length,
            lotsApplied  = allLots.Count,
            parseErrors  = errors
        });
    }

    // ── Mapping helpers ───────────────────────────────────────────────────────

    private static StorageDto MapToDto(StoreStorage storage)
    {
        var cabinets = storage.Cabinets
            .Select((cab, cabIdx) => MapCabinetToDto(cab, cabIdx))
            .ToList()
            .AsReadOnly();

        return new StorageDto(cabinets);
    }

    private static CabinetDto MapCabinetToDto(Cabinet cabinet, int cabIdx)
    {
        // Collect all containers across all groups so we can build sequential IDs
        // that match the database container numbers.
        var groups = cabinet.Groups
            .Select((grp, grpIdx) => MapGroupToDto(grp, cabIdx, grpIdx))
            .ToList()
            .AsReadOnly();

        return new CabinetDto(cabIdx + 1, $"Cabinet {(char)('A' + cabIdx)}", groups);
    }

    private static CaseGroupDto MapGroupToDto(CaseGroup group, int cabIdx, int grpIdx)
    {
        var firstCase       = group.Cases.FirstOrDefault();
        var containerType   = firstCase?.ContainerType.ToString() ?? "PX6";
        var label           = $"Case Group {cabIdx * 4 + grpIdx + 1}";

        var containers = group.Cases
            .SelectMany(c => c.Containers)
            .Select((con, conIdx) => MapContainerToDto(con, conIdx))
            .ToList()
            .AsReadOnly();

        return new CaseGroupDto(grpIdx + 1, label, containerType, containers);
    }

    private static ContainerDto MapContainerToDto(Container container, int index)
    {
        var sections = container.Sections
            .Select((s, i) => new SectionDto(i, s.IsEmpty, s.LotId, s.Quantity))
            .ToList()
            .AsReadOnly();

        int emptySections = sections.Count(s => s.IsEmpty);

        return new ContainerDto(
            index + 1,
            container.Sections.Count,
            emptySections,
            sections);
    }
}
