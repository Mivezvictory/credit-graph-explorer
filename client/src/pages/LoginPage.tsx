import { Box, Container, Typography, CssBaseline } from '@mui/material';
import { ThemeProvider } from '@emotion/react';
import { theme } from '../stylings/theme';
import {  LOGIN_PAGE_STYLING, 
          SEMI_TRANSPARENT_OVERLAY,
          SINGIN_CARD_STYLING
      } from './pageStyling/pageTheme';

import { LoginButton } from '../components/LoginButton';

export function LoginPage() {
  return (
    <ThemeProvider theme={{theme}}>
      <Box sx={LOGIN_PAGE_STYLING}>
        <CssBaseline />

        {/* Semi-transparent overlay */}
        <Box
          sx={SEMI_TRANSPARENT_OVERLAY}/>

        {/* Sign-in Card Container */}
        <Container
          component="main"
          maxWidth="xs"
          disableGutters
          sx={SINGIN_CARD_STYLING}
        >
          <Box
            component="img"
            src="/icon.svg"
            alt="Credit Graph Explorer Logo"
            sx={{ width: { xs: 80, sm: 128 }, mb: 3 }}
          />
        <Typography component="h1" variant="h5" sx={{ mb: 2, fontWeight: 'bold' }}>
            Credit Graph Explorer
          </Typography>
          <LoginButton
            buttonDescription="Sign in with Spotify"
          />
          <Typography variant="caption" sx={{ mt: 2, color: 'white' }}>
            Powered by Spotify Web API
          </Typography>
        </Container>
      </Box>
    </ThemeProvider>

  );
}
