import { createSlice, createAsyncThunk, type PayloadAction } from '@reduxjs/toolkit';
import type { StorageDto } from '../Interfaces/storage';
import { fetchStorage, updateStorageState } from '../Api/storageApi';

// ── Thunks ────────────────────────────────────────────────────────────────────

export const loadStorage = createAsyncThunk('storage/load', async () => {
  return fetchStorage();
});

export const triggerUpdateState = createAsyncThunk('storage/updateState', async () => {
  return updateStorageState();
});

// ── State ─────────────────────────────────────────────────────────────────────

interface StorageState {
  data: StorageDto | null;
  /** containerIndex -> set of section indices marked full in UI */
  localFullSections: Record<string, boolean>;
  loading: boolean;
  updating: boolean;
  error: string | null;
  updateMessage: string | null;
}

const initialState: StorageState = {
  data: null,
  localFullSections: {},
  loading: false,
  updating: false,
  error: null,
  updateMessage: null,
};

// ── Slice ─────────────────────────────────────────────────────────────────────

const storageSlice = createSlice({
  name: 'storage',
  initialState,
  reducers: {
    /** Mark an empty section as Full in the UI (no backend call). */
    markSectionFull(
      state,
      action: PayloadAction<{ containerId: number; sectionIndex: number }>
    ) {
      const key = `${action.payload.containerId}-${action.payload.sectionIndex}`;
      state.localFullSections[key] = true;
    },
    clearUpdateMessage(state) {
      state.updateMessage = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(loadStorage.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(loadStorage.fulfilled, (state, action) => {
        state.loading = false;
        state.data = action.payload;
        state.localFullSections = {};
      })
      .addCase(loadStorage.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message ?? 'Failed to load storage.';
      })
      .addCase(triggerUpdateState.pending, (state) => {
        state.updating = true;
        state.updateMessage = null;
        state.error = null;
      })
      .addCase(triggerUpdateState.fulfilled, (state, action) => {
        state.updating = false;
        state.updateMessage = action.payload.message;
      })
      .addCase(triggerUpdateState.rejected, (state, action) => {
        state.updating = false;
        state.error = action.error.message ?? 'Update failed.';
      });
  },
});

export const { markSectionFull, clearUpdateMessage } = storageSlice.actions;
export default storageSlice.reducer;
