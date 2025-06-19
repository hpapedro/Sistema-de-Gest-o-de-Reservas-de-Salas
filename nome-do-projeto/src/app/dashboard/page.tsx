'use client';

import { useRouter } from 'next/navigation';
import { Box, Button, Container, Typography, Stack, Paper } from '@mui/material';

export default function DashboardPage() {
  const router = useRouter();

  return (
    <Container maxWidth="sm" sx={{ mt: 5 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h4" gutterBottom align="center">
          Selecione uma opção
        </Typography>

        <Stack spacing={2} mt={4}>
          <Button
            variant="contained"
            color="primary"
            onClick={() => router.push('/salas')}
            fullWidth
          >
            Gerenciar Salas
          </Button>

          <Button
            variant="contained"
            color="secondary"
            onClick={() => router.push('/reserva')}
            fullWidth
          >
            Gerenciar Reservas
          </Button>

          <Button
            variant="contained"
            color="info"
            onClick={() => router.push('/auditoria')}
            fullWidth
          >
            Visualizar Auditoria
          </Button>
        </Stack>
      </Paper>
    </Container>
  );
}
