import { createTheme } from '@mui/material/styles';
// export const LOGIN_PAGE_STYLING = {
//     minHeight: '70vh',
//     textAlign: 'center',
//     backgroundImage: 'url(../../public/images/bg-image.jpg)', // <-- Add this line
//     backgroundSize: 'cover',
//     backgroundRepeat: 'no-repeat',
//     backgroundPosition: 'center',
// }
export const LOGIN_PAGE_STYLING = {
    height: '70vh',
    textAlign: 'center'
    
};

export const APP_LAYOUT_BG_STYLE = {
    minHeight: '100vh',
    display: 'flex',
    flexDirection: 'column',
   
};

export const LOGIN_BACKGROUND = { 
    flex: 1, 
    p: 2, 
    bgcolor: 'background.default' ,
     
}

export const theme = createTheme({
  palette: {
    primary: { main: '#1db954' },
    secondary: { main: '#191414' },
    background: { default: '#fafafa' },
  },
  typography: {
    fontFamily: 'Roboto, Arial, sans-serif',
  },
  // Add component overrides here if needed
});

