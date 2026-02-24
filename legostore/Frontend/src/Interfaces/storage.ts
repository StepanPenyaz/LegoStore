export interface SectionDto {
  index: number;
  isEmpty: boolean;
  lotId: string | null;
  quantity: number;
}

export interface ContainerDto {
  id: number;
  totalSections: number;
  emptySections: number;
  sections: SectionDto[];
}

export interface CaseGroupDto {
  id: number;
  label: string;
  containerType: string;
  containers: ContainerDto[];
}

export interface CabinetDto {
  id: number;
  name: string;
  caseGroups: CaseGroupDto[];
}

export interface StorageDto {
  cabinets: CabinetDto[];
}

export interface UpdateStateResult {
  message: string;
  filesCount: number;
  lotsApplied: number;
  parseErrors: string[];
}
