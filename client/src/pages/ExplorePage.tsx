import { useState } from 'react';
import { Button, Card, CardActionArea, CardContent, CardMedia, Stack, TextField, Typography } from '@mui/material';
import { buildArtistGraph, getGraphRoutes, createPlaylist } from '../services/backend';
import { searchArtists } from '../services/spotify';
import { useAuth } from '../providers/AuthProvider';
import { ThemeProvider } from '@emotion/react';
import { theme } from '../stylings/theme'

export default function ExplorePage() {
  const { token } = useAuth();
  const [q, setQ] = useState('');
  const [artists, setArtists] = useState<any[]>([]);
  const [graph, setGraph] = useState<any>(null);
  const [routes, setRoutes] = useState<any[]>([]);
  const [selectedRoute, setSelectedRoute] = useState<any>(null);
  const [loading, setLoading] = useState(false);

  const onSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!token) return;
    const items = await searchArtists(token, q);
    setArtists(items);
  };

  const onBuildGraph = async (artist: any) => {
    if (!token) return;
    setLoading(true);
    try {
      setGraph(artist);
      const graphData = await buildArtistGraph(token, artist.id);
      const routesData = await getGraphRoutes(token, graphData.id);
      setRoutes(routesData.routes || []);
    } catch (err) {
      console.error('Error building graph:', err);
      setRoutes([]);
    } finally {
      setLoading(false);
    }
  };

  return (
    <ThemeProvider theme={{theme}}>
      <Stack spacing={3}>
        <form onSubmit={onSearch}>
          <Stack direction="row" spacing={2}>
            <TextField fullWidth label="Search artist…" value={q} onChange={e => setQ(e.target.value)} />
            <Button type="submit" variant="outlined">Search</Button>
          </Stack>
        </form>

        <Stack spacing={2}>
          {artists.map(a => {
            const img = a.images?.[0]?.url;
            return (
              <Card key={a.id} variant="outlined">
                <CardActionArea onClick={() => onBuildGraph(a)}>
                  <Stack direction="row" spacing={2} alignItems="center">
                    {img && <CardMedia component="img" image={img} alt={a.name} sx={{ width: 90, height: 90 }} />}
                    <CardContent sx={{ flex: 1 }}>
                      <Typography variant="h6">{a.name}</Typography>
                      {!!a.followers?.total && (
                        <Typography variant="body2" color="text.secondary">
                          {a.followers.total.toLocaleString()} followers
                        </Typography>
                      )}
                      <Typography variant="body2" color="primary">Click to explore artist graph</Typography>
                    </CardContent>
                  </Stack>
                </CardActionArea>
              </Card>
            );
          })}
        </Stack>

        {graph && routes.length > 0 && (
          <Stack spacing={2}>
            <Typography variant="h5">Artist Paths for {graph.name}</Typography>
            {routes.map((route, idx) => (
              <Card key={idx} variant="outlined">
                <CardContent>
                  <Typography fontWeight={600}>{route.name || `Route ${idx + 1}`}</Typography>
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                    {route.description || 'Artist route through the graph'}
                  </Typography>
                  <Button 
                    variant="contained" 
                    size="small" 
                    onClick={() => setSelectedRoute(route)}
                    sx={{ textTransform: 'none' }}
                  >
                    View Route
                  </Button>
                </CardContent>
              </Card>
            ))}
          </Stack>
        )}
      </Stack>
    </ThemeProvider>
  );
}

function getId(uri: string) {
  if (!uri) return '';
  if (uri.startsWith('spotify:')) return uri.split(':').pop() ?? '';
  try { const u = new URL(uri); const parts = u.pathname.split('/').filter(Boolean); const i = parts.indexOf('track'); return i >= 0 ? parts[i+1] : ''; } catch { return uri; }
}
