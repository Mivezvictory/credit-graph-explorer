import { Box, Container, Typography, CssBaseline, Stack } from '@mui/material';
import { ThemeProvider } from '@emotion/react';
import { theme } from '../stylings/theme';
import { LOGIN_PAGE_STYLING, SEMI_TRANSPARENT_OVERLAY, SINGIN_CARD_STYLING } from './pageStyling/pageTheme';
import BuildIcon from '@mui/icons-material/Build';
import { keyframes } from '@emotion/react';

// Animations
const float = keyframes`
  0%, 100% { transform: translateY(0px); }
  50% { transform: translateY(-20px); }
`;

const pulse = keyframes`
  0%, 100% { opacity: 1; }
  50% { opacity: 0.7; }
`;

const gradientShift = keyframes`
  0%, 100% { background-position: 0% 50%; }
  50% { background-position: 100% 50%; }
`;

export function UnderConstructionPage() {
  return (
    <ThemeProvider theme={{ theme }}>
      <Box sx={LOGIN_PAGE_STYLING}>
        <CssBaseline />

        {/* Semi-transparent overlay */}
        <Box sx={SEMI_TRANSPARENT_OVERLAY} />

        {/* Animated background */}
        <Box
          sx={{
            position: 'absolute',
            top: 0,
            left: 0,
            right: 0,
            bottom: 0,
            background: 'linear-gradient(-45deg, rgba(29, 185, 84, 0.1), rgba(191, 90, 242, 0.1), rgba(30, 215, 96, 0.1))',
            backgroundSize: '400% 400%',
            animation: `${gradientShift} 15s ease infinite`,
            pointerEvents: 'none',
          }}
        />

        {/* Under Construction Card Container */}
        <Container
          component="main"
          maxWidth="xs"
          disableGutters
          sx={{
            ...SINGIN_CARD_STYLING,
            position: 'relative',
            zIndex: 1,
            background: 'rgba(18, 18, 18, 0.8)',
            backdropFilter: 'blur(10px)',
            border: '1px solid rgba(29, 185, 84, 0.2)',
            borderRadius: '16px',
            boxShadow: '0 8px 32px 0 rgba(31, 38, 135, 0.37)',
          }}
        >
          {/* Floating Build Icon */}
          <Box
            sx={{
              width: { xs: 80, sm: 128 },
              mb: 3,
              display: 'flex',
              justifyContent: 'center',
              animation: `${float} 3s ease-in-out infinite`,
            }}
          >
            <BuildIcon
              sx={{
                fontSize: { xs: 80, sm: 128 },
                color: '#1DB954',
                filter: 'drop-shadow(0 0 20px rgba(29, 185, 84, 0.5))',
              }}
            />
          </Box>

          {/* Title with gradient */}
          <Typography
            component="h1"
            variant="h5"
            sx={{
              mb: 2,
              fontWeight: 'bold',
              textAlign: 'center',
              color: 'white',
              fontSize: { xs: '1.8rem', sm: '2.2rem' },
              letterSpacing: '0.5px',
            }}
          >
            Credit Graph Explorer
          </Typography>

          {/* Under Construction Badge */}
          <Box
            sx={{
              display: 'inline-block',
              mx: 'auto',
              mb: 3,
              px: 2,
              py: 1,
              background: 'linear-gradient(135deg, rgba(29, 185, 84, 0.2) 0%, rgba(191, 90, 242, 0.2) 100%)',
              border: '1px solid rgba(29, 185, 84, 0.4)',
              borderRadius: '24px',
              animation: `${pulse} 2s ease-in-out infinite`,
            }}
          >
            <Typography
              variant="body2"
              sx={{
                textAlign: 'center',
                color: '#1DB954',
                fontWeight: '600',
              }}
            >
              🚧 Under Construction
            </Typography>
          </Box>

          {/* Description */}
          <Typography
            variant="body2"
            sx={{
              textAlign: 'center',
              color: 'rgba(255, 255, 255, 0.85)',
              lineHeight: 1.8,
              mb: 2,
              fontSize: '0.95rem',
            }}
          >
            We're building something amazing! This application is currently under development.
          </Typography>

          <Typography
            variant="body2"
            sx={{
              textAlign: 'center',
              color: 'rgba(255, 255, 255, 0.85)',
              lineHeight: 1.8,
              mb: 3,
              fontSize: '0.95rem',
            }}
          >
            Check back soon to explore artist relationships and discover new music through the credit graph.
          </Typography>

          {/* Features coming soon */}
          <Stack
            spacing={1}
            sx={{
              mb: 3,
              px: 2,
              py: 2,
              background: 'rgba(29, 185, 84, 0.05)',
              borderRadius: '8px',
              border: '1px solid rgba(29, 185, 84, 0.1)',
            }}
          >
            <Typography
              variant="caption"
              sx={{
                color: '#1DB954',
                fontWeight: '600',
                textTransform: 'uppercase',
                letterSpacing: '1px',
              }}
            >
              Coming Soon
            </Typography>
            <Typography variant="caption" sx={{ color: 'rgba(255, 255, 255, 0.7)' }}>
              ✓ Artist Graph Exploration
            </Typography>
            <Typography variant="caption" sx={{ color: 'rgba(255, 255, 255, 0.7)' }}>
              ✓ Producer Trail Routes
            </Typography>
            <Typography variant="caption" sx={{ color: 'rgba(255, 255, 255, 0.7)' }}>
              ✓ Playlist Generation
            </Typography>
          </Stack>

          {/* Footer */}
          <Typography
            variant="caption"
            sx={{
              display: 'block',
              color: 'rgba(255, 255, 255, 0.6)',
              textAlign: 'center',
              fontSize: '0.8rem',
            }}
          >
            Powered by Spotify Web API, MusicBrainz, and Last.fm
          </Typography>
        </Container>
      </Box>
    </ThemeProvider>
  );
}
