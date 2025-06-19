'use client';

import { AppBar, Toolbar, Typography, Button, Stack } from '@mui/material';
import { useRouter } from 'next/navigation';

export default function Header() {
  const router = useRouter();

  const temToken = typeof window !== 'undefined' && !!localStorage.getItem('token');

  const sair = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    router.push('/login');
  };

  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" sx={{ flexGrow: 1 }}>
          Sistema de Reservas
        </Typography>

        {temToken && (
          <Stack direction="row" spacing={2}>
            <Button color="inherit" onClick={() => router.push('/dashboard')}>
              Home
            </Button>
            <Button color="inherit" onClick={sair}>
              Sair
            </Button>
          </Stack>
        )}
      </Toolbar>
    </AppBar>
  );
}
