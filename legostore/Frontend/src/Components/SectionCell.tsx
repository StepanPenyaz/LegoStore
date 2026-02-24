import { useDispatch, useSelector } from 'react-redux';
import type { SectionDto } from '../Interfaces/storage';
import type { AppDispatch, RootState } from '../Store/store';
import { markSectionFull } from '../Store/storageSlice';

interface Props {
  section: SectionDto;
  containerId: number;
}

export function SectionCell({ section, containerId }: Props) {
  const dispatch = useDispatch<AppDispatch>();
  const key = `${containerId}-${section.index}`;
  const isLocallyFull = useSelector(
    (s: RootState) => s.storage.localFullSections[key] === true
  );

  const isEmpty = section.isEmpty && !isLocallyFull;

  const handleClick = () => {
    if (isEmpty) {
      dispatch(markSectionFull({ containerId, sectionIndex: section.index }));
    }
  };

  const title = isEmpty
    ? `Section ${section.index + 1}: empty — click to mark full`
    : `Section ${section.index + 1}: ${section.lotId ?? 'full'} (qty ${section.quantity})`;

  return (
    <div
      className={`section-cell ${isEmpty ? 'section-empty' : 'section-full'}`}
      onClick={handleClick}
      title={title}
      role="button"
      aria-label={title}
    />
  );
}
