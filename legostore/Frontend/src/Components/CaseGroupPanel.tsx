import type { CaseGroupDto } from '../Interfaces/storage';
import { ContainerCell } from './ContainerCell';

// Max grid columns per container type
const GRID_COLS: Record<string, number> = {
  PX12: 12,
  PX6: 6,
  PX4: 6,
  PX2: 6,
};

interface Props {
  group: CaseGroupDto;
  /** Starting container number for this group (for the #XXXX labels) */
  startContainerNumber: number;
}

export function CaseGroupPanel({ group, startContainerNumber }: Props) {
  const cols = GRID_COLS[group.containerType] ?? 6;

  return (
    <div className="case-group">
      <div className="case-group-title">
        {group.label} &mdash; {group.containerType}
      </div>
      <div
        className="case-group-grid"
        style={{ gridTemplateColumns: `repeat(${cols}, auto)` }}
      >
        {group.containers.map((con, idx) => (
          <ContainerCell
            key={con.id}
            container={con}
            containerNumber={startContainerNumber + idx}
          />
        ))}
      </div>
    </div>
  );
}
