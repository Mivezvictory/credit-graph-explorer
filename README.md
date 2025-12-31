# Credit Graph Explorer

A music discovery application that builds artist relationship graphs and creates curated Spotify playlists based on artist connections and production credits.

## Overview

Credit Graph Explorer is designed to help music enthusiasts explore the intricate web of relationships between artists, producers, and collaborators. By visualizing these connections as a graph, users can discover new music and explore how artists are connected through their work.

## Features

- **Artist Graph Building**: Automatically construct relationship graphs starting from any Spotify artist
- **Multi-Source Data Integration**: Combines data from:
  - **Spotify API** - Artist details, albums, and tracks
  - **Last.fm API** - Related artists and listening patterns
  - **MusicBrainz** - Production credits and metadata
- **Route Generation**: Create curated paths through the artist graph
  - **Producer Trail (Beta)** - Trace artists by their collaborative relationships
- **Playlist Creation**: Convert artist routes into private Spotify playlists with representative tracks from each artist
- **OAuth Authentication**: Secure Spotify login for playlist creation

## Architecture

### Backend
Built with **Azure Functions** and **.NET 8** for serverless scalability:

- **CreditGraph.Functions** - HTTP-triggered Azure Functions endpoints
- **CreditGraph.Services** - Core business logic and interfaces
- **CreditGraph.Infrastructure** - API client implementations (Spotify, Last.fm, MusicBrainz)
- **CreditGraph.Domain** - Domain models (Artist, Album, ArtistGraph, Relation)
- **CreditGraph.Options** - Configuration management

### Frontend
Built with **React**, **TypeScript**, and **Vite**:

- **Modern React Stack** - React 19 with TypeScript
- **Material-UI Components** - Professional UI components
- **Emotion Styling** - CSS-in-JS styling solution
- **Vite** - Lightning-fast build tool and dev server

### Key APIs

**Graph Building**
- `GET /api/graph?artistId={id}` - Build artist relationship graph

**Route Generation**
- `GET /api/graph/{graphId}/routes` - Get available routes through the graph

**Playlist Creation**
- `POST /api/create-playlist` - Generate Spotify playlist from route

**Authentication**
- `GET /api/spotify-auth` - Initiate OAuth flow
- `GET /api/spotify-callback` - Handle OAuth redirect

## Getting Started

### Prerequisites

- **Backend**: .NET 8 SDK, Azure Functions Core Tools
- **Frontend**: Node.js 18+, npm or yarn
- **APIs**: Spotify Developer Account, Last.fm API Key, MusicBrainz Account

### Installation

#### Backend Setup

```bash
cd server
dotnet build
# Configure CreditGraph.Functions/local.settings.json with API keys
func start
```

#### Frontend Setup

```bash
cd client
npm install
cp .env.local.example .env.local
# Update VITE_API_BASE_URL in .env.local
npm run dev
```

### Environment Configuration

**Backend** (`server/CreditGraph.Functions/local.settings.json`):
```json
{
  "SpotifyClientId": "your-spotify-client-id",
  "SpotifyClientSecret": "your-spotify-client-secret",
  "LastFmApiKey": "your-lastfm-api-key",
  "MusicBrainzApiKey": "your-musicbrainz-api-key"
}
```

**Frontend** (`client/.env.local`):
```
VITE_API_BASE_URL=https://localhost:7071/api
```

## Project Status

**Currently Under Development**

### Completed
- Basic project structure and scaffolding
- Azure Functions setup
- Frontend React application
- OAuth authentication flow
- Spotify API integration
- UI components and theming

### In Progress
- Graph building algorithm
- Route generation (Producer Trail)
- Last.fm integration
- MusicBrainz integration

### Planned
- Advanced routing algorithms
- Graph visualization
- Playlist sharing
- User history and recommendations

## Tech Stack

### Backend
- **.NET 8** - Runtime
- **Azure Functions** - Serverless compute
- **C#** - Language
- **REST API** - Architecture

### Frontend
- **React 19** - UI framework
- **TypeScript** - Type safety
- **Vite** - Build tool
- **Material-UI** - Component library
- **Emotion** - CSS-in-JS
- **React Router** - Routing

### External Services
- **Spotify Web API** - Music data and authentication
- **Last.fm API** - Artist relationships
- **MusicBrainz API** - Production metadata

## Development

### Running Tests

```bash
# Backend
cd server
dotnet test

# Frontend
cd client
npm run test
```

### Building

```bash
# Backend
cd server
dotnet build -c Release

# Frontend
cd client
npm run build
```

### Linting

```bash
# Frontend
cd client
npm run lint
```
## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- **Spotify** - For the comprehensive Web API
- **Last.fm** - For artist relationship data
- **MusicBrainz** - For production metadata
- **Material-UI** - For the excellent component library

## Contact

For questions or suggestions, please reach out to [my email](mailto:mivez70@gmail.com)

---
