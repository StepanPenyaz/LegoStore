import type { StorageDto, UpdateStateResult } from '../Interfaces/storage';

const BASE_URL = '/api/storage';

export async function fetchStorage(): Promise<StorageDto> {
  const response = await fetch(BASE_URL);
  if (!response.ok) {
    throw new Error(`Failed to fetch storage: ${response.statusText}`);
  }
  return response.json() as Promise<StorageDto>;
}

export async function updateStorageState(): Promise<UpdateStateResult> {
  const response = await fetch(`${BASE_URL}/update-state`, { method: 'POST' });
  if (!response.ok) {
    const text = await response.text();
    throw new Error(`Update failed: ${text}`);
  }
  return response.json() as Promise<UpdateStateResult>;
}
