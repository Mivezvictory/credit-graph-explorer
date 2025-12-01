export async function searchArtists(token: string, q: string) {
  const r = await fetch(`https://api.spotify.com/v1/search?type=artist&limit=10&q=${encodeURIComponent(q)}`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  if (!r.ok) throw new Error(`Search failed (${r.status})`);
  const data = await r.json();
  return (data?.artists?.items ?? []).map((a: any) => ({
    id: a.id, name: a.name, images: a.images, followers: a.followers
  }));
}

export async function getTracksByIds(token: string, ids: string[]) {
  const r = await fetch(`https://api.spotify.com/v1/tracks?ids=${ids.join(',')}`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  if (!r.ok) throw new Error(`Track lookup failed (${r.status})`);
  const data = await r.json();
  return data?.tracks ?? [];
}
