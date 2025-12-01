import { createContext, useContext, useEffect, useMemo, useState } from 'react';
const apiBase = import.meta.env.VITE_API_BASE_URL as string;

type Ctx = { token: string | null; isAuthed: boolean; login: () => void; logout: () => void };
const AuthCtx = createContext<Ctx | null>(null);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [token, setToken] = useState<string | null>(null);

  useEffect(() => {
    const params = new URLSearchParams(window.location.search);
    const t = params.get('token');
    if (t) {
      localStorage.setItem('spotify_token', t);
      setToken(t);
      window.history.replaceState({}, '', window.location.pathname);
      return;
    }
    const stored = localStorage.getItem('spotify_token');
    if (stored) setToken(stored);
  }, []);

  const login = () => { window.location.href = `${apiBase}/spotify-auth`; };
  const logout = () => { localStorage.removeItem('spotify_token'); setToken(null); };

  const value = useMemo(() => ({ token, isAuthed: !!token, login, logout }), [token]);
  return <AuthCtx.Provider value={value}>{children}</AuthCtx.Provider>;
}
export const useAuth = () => {
  const v = useContext(AuthCtx);
  if (!v) throw new Error('useAuth must be used within AuthProvider');
  return v;
};
