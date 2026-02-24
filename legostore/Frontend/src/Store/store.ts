import { configureStore } from '@reduxjs/toolkit';
import storageReducer from './storageSlice';

export const store = configureStore({
  reducer: {
    storage: storageReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
