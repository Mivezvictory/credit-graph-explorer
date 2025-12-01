import { createContext, useContext, useMemo, useState } from 'react';
import { ThemeProvider, CssBaseline, createTheme } from '@mui/material';

type Mode = 'light' | 'dark';
const Ctx = createContext<{ mode: Mode; toggle: () => void } | null>(null);

export function ThemeModeProvider({ children }: { children: React.ReactNode }) {
  const [mode, setMode] = useState<Mode>(() => (localStorage.getItem('theme') as Mode) || 'light');
  const toggle = () => setMode(m => (m === 'light' ? 'dark' : 'light'));
  const theme = useMemo(() => createTheme({
    palette: {
      mode,
      primary: { main: '#1DB954' },
      ...(mode === 'dark'
        ? { background: { default: '#121212', paper: '#1e1e1e' } }
        : { background: { default: '#fafafa', paper: '#fff' } })
    }
  }), [mode]);

  // persist
  useMemo(() => localStorage.setItem('theme', mode), [mode]);

  return (
    <Ctx.Provider value={{ mode, toggle }}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        {children}
      </ThemeProvider>
    </Ctx.Provider>
  );
}
export const useThemeMode = () => {
  const v = useContext(Ctx);
  if (!v) throw new Error('useThemeMode must be used within ThemeModeProvider');
  return v;
};
