const apiBase = import.meta.env.VITE_API_BASE_URL as string;

export async function buildArtistGraph(token: string, artistId: string): Promise<any> {
  const res = await fetch(`${apiBase}/graph?artistId=${artistId}`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  if (!res.ok) throw new Error(`Failed to build artist graph (${res.status})`);
  return res.json();
}

export async function getGraphRoutes(token: string, graphId: string): Promise<any> {
  const res = await fetch(`${apiBase}/graph/${graphId}/routes`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  if (!res.ok) throw new Error(`Failed to fetch graph routes (${res.status})`);
  return res.json();
}

export async function createPlaylist(token: string, routeId: string, playlistName: string): Promise<any> {
  const res = await fetch(`${apiBase}/create-playlist`, {
    method: 'POST',
    headers: { 
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({ routeId, playlistName })
  });
  if (!res.ok) throw new Error(`Failed to create playlist (${res.status})`);
  return res.json();
}
