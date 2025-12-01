import { ThemeProvider } from "@emotion/react";
import { theme } from "../stylings/theme";
import { Button } from "@mui/material";
import { useAuth } from "../providers/AuthProvider";

export const LoginButton = ({buttonDescription}: ButtonProps) => {
    const { login } = useAuth();
    return (
         <ThemeProvider theme={{theme}}>
            <Button
            fullWidth
            variant="contained"
            onClick={login}
            sx={{
              mt: 1,
              backgroundColor: '#1DB954',
              color: 'black',
              py: 1.5,
              '&:hover': { backgroundColor: '#1ED760' },
            }}
          >
           {buttonDescription}
          </Button>
         </ThemeProvider>
    );
}

type ButtonProps = {
  buttonDescription: string;
};