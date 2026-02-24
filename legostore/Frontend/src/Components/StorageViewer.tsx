import { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import type { AppDispatch, RootState } from '../Store/store';
import { loadStorage, triggerUpdateState, clearUpdateMessage } from '../Store/storageSlice';
import { CabinetPanel } from './CabinetPanel';

export function StorageViewer() {
  const dispatch = useDispatch<AppDispatch>();
  const { data, loading, updating, error, updateMessage } = useSelector(
    (s: RootState) => s.storage
  );
  const [activeCabId, setActiveCabId] = useState<number | null>(null);

  useEffect(() => {
    dispatch(loadStorage());
  }, [dispatch]);

  useEffect(() => {
    if (data && data.cabinets.length > 0 && activeCabId === null) {
      setActiveCabId(data.cabinets[0].id);
    }
  }, [data, activeCabId]);

  const handleUpdateState = async () => {
    await dispatch(triggerUpdateState());
    dispatch(loadStorage());
  };

  const handleDismissMessage = () => dispatch(clearUpdateMessage());

  return (
    <>
      <header>
        <span className="logo">🧱</span>
        <h1>LegoStore – Storage Viewer</h1>
        <div className="header-actions">
          <button
            className="btn-update"
            onClick={handleUpdateState}
            disabled={updating || loading}
          >
            {updating ? 'Updating…' : 'Update Storage State'}
          </button>
        </div>
      </header>

      {(error || updateMessage) && (
        <div className={`notification ${error ? 'notification-error' : 'notification-success'}`}>
          {error ?? updateMessage}
          <button className="notification-close" onClick={handleDismissMessage}>✕</button>
        </div>
      )}

      <div className="legend">
        <strong>Legend:</strong>
        <div className="legend-item">
          <div className="legend-swatch swatch-empty" />
          Fully empty
        </div>
        <div className="legend-item">
          <div className="legend-swatch swatch-partial" />
          Partially empty
        </div>
        <div className="legend-item">
          <div className="legend-swatch swatch-full" />
          Fully occupied
        </div>
        <div className="legend-item">
          <div className="legend-swatch swatch-section-empty" />
          Empty section (click to mark full)
        </div>
      </div>

      {loading && <div className="loading-msg">Loading storage data…</div>}

      {!loading && data && (
        <>
          <div className="tabs-bar">
            {data.cabinets.map((cab) => (
              <button
                key={cab.id}
                className={`tab-btn ${activeCabId === cab.id ? 'active' : ''}`}
                onClick={() => setActiveCabId(cab.id)}
              >
                {cab.name}
              </button>
            ))}
          </div>

          <div id="panels-container">
            {data.cabinets.map((cab) => (
              <CabinetPanel
                key={cab.id}
                cabinet={cab}
                isActive={cab.id === activeCabId}
              />
            ))}
          </div>
        </>
      )}

      {!loading && !data && !error && (
        <div className="loading-msg">No storage data found. Please initialise the database.</div>
      )}
    </>
  );
}
