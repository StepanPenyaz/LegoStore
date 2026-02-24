import { useSelector } from 'react-redux';
import type { ContainerDto } from '../Interfaces/storage';
import type { RootState } from '../Store/store';
import { SectionCell } from './SectionCell';

interface Props {
  container: ContainerDto;
  /** Absolute container number (e.g. 1001) */
  containerNumber: number;
}

export function ContainerCell({ container, containerNumber }: Props) {
  const localFullSections = useSelector((s: RootState) => s.storage.localFullSections);

  // Count how many sections are effectively empty (considering local overrides)
  const effectiveEmpty = container.sections.filter((sec) => {
    const key = `${container.id}-${sec.index}`;
    return sec.isEmpty && !localFullSections[key];
  }).length;

  const total = container.totalSections;

  let colorClass = '';
  if (effectiveEmpty === total) {
    colorClass = 'fully-empty';
  } else if (effectiveEmpty > 0) {
    colorClass = 'partially-empty';
  }

  const fillPct = Math.round((effectiveEmpty / total) * 100);
  const label = `#${String(containerNumber).padStart(4, '0')}`;

  return (
    <div className="container-cell">
      <div className={`container-rect ${colorClass}`} title={`${label}: ${effectiveEmpty}/${total} empty`}>
        {colorClass === 'partially-empty' && (
          <div className="fill" style={{ width: `${fillPct}%` }} />
        )}
        <div className="sections-overlay">
          {container.sections.map((sec) => (
            <SectionCell key={sec.index} section={sec} containerId={container.id} />
          ))}
        </div>
      </div>
      <span className="container-label">{label}</span>
    </div>
  );
}
