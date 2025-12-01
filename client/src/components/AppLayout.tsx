import type { ReactNode } from 'react';
import { AppBar, Box, IconButton, Toolbar, Typography } from '@mui/material';
import Brightness4 from '@mui/icons-material/Brightness4';
import Brightness7 from '@mui/icons-material/Brightness7';
import { useThemeMode } from '../providers/ThemeModeProvider';
import { useAuth } from '../providers/AuthProvider';
import { APP_LAYOUT_BG_STYLE, LOGIN_BACKGROUND } from '../stylings/theme';
import { ThemeProvider } from '@emotion/react';
import { theme } from '../stylings/theme';


export default function AppLayout({ children }: { children: ReactNode }) {
  const { mode, toggle } = useThemeMode();
  const { isAuthed, logout } = useAuth();

  return (
    <ThemeProvider theme={{theme}}>
      <Box sx={APP_LAYOUT_BG_STYLE}>
        <AppBar position="static" color="primary">
          <Toolbar sx={{ justifyContent: 'space-between' }}>
            <Typography variant="h6">Credit Graph Explorer</Typography>
            <Box>
              {isAuthed && (
                <IconButton onClick={logout} color="inherit" sx={{ mr: 1 }} aria-label="Sign out">
                  <Typography variant="body2">Sign out</Typography>
                </IconButton>
              )}
              <IconButton onClick={toggle} color="inherit" aria-label="Toggle theme">
                {mode === 'dark' ? <Brightness7 /> : <Brightness4 />}
              </IconButton>
            </Box>
          </Toolbar>
        </AppBar>

        <Box component="main" sx={LOGIN_BACKGROUND}>
          {children}
        </Box>

        <Box component="footer" sx={{ p: 1, textAlign: 'center', bgcolor: 'background.paper' }}>
          Powered by Spotify Web API
        </Box>
      </Box>
    </ThemeProvider>
  );
}
