import type { CabinetDto } from '../Interfaces/storage';
import { CaseGroupPanel } from './CaseGroupPanel';

interface Props {
  cabinet: CabinetDto;
  isActive: boolean;
}

export function CabinetPanel({ cabinet, isActive }: Props) {
  // Compute a global container offset so each group starts numbering after the previous one
  let containerOffset = 1;

  return (
    <div className={`cabinet-panel ${isActive ? 'active' : ''}`} id={`panel-cab-${cabinet.id}`}>
      {cabinet.caseGroups.map((group) => {
        const startNum = containerOffset;
        containerOffset += group.containers.length;
        return (
          <CaseGroupPanel
            key={group.id}
            group={group}
            startContainerNumber={startNum}
          />
        );
      })}
    </div>
  );
}
