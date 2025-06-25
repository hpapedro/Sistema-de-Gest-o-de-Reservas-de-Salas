'use client';

import { useState } from 'react';
import {
  Button,
  Container,
  Typography,
  Alert,
  Stack,
  Paper,
  List,
  ListItem,
  ListItemText,
  Divider,
  CircularProgress,
} from '@mui/material';

interface LogAuditoria {
  id: number;
  usuarioId: number;
  usuarioEmail: string;
  reservaId: number;
  dataHoraAcao: string;
  acao: string; // ex: "Criada", "Excluída"
}

const urlBase = 'http://localhost:5000/api';

export default function AuditoriasPage() {
  const [logs, setLogs] = useState<LogAuditoria[]>([]);
  const [erro, setErro] = useState('');
  const [carregando, setCarregando] = useState(false);
  const [tipoLog, setTipoLog] = useState<'criadas' | 'excluidas' | null>(null);

  async function fetchAuditoria(tipo: 'criadas' | 'excluidas') {
    setErro('');
    setLogs([]);
    setTipoLog(tipo);
    setCarregando(true);

    const token = localStorage.getItem('token');
    if (!token) {
      setErro('Token não encontrado. Faça login.');
      setCarregando(false);
      return;
    }

    try {
      const resposta = await fetch(
        `${urlBase}/auditoria/reservas/${tipo}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (!resposta.ok) {
        const text = await resposta.text();
        throw new Error(`Erro HTTP ${resposta.status} - ${text}`);
      }

      const dados = await resposta.json();

      // Se os dados vierem com $values (Entity Framework), adapta
      const listaLogs = Array.isArray(dados) ? dados : dados.$values || [];
      setLogs(listaLogs);
    } catch (e: any) {
      setErro(e.message || 'Erro ao buscar logs de auditoria.');
    } finally {
      setCarregando(false);
    }
  }

  return (
    <Container maxWidth="md" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h4" gutterBottom align="center">
        Logs de Auditoria - Reservas
      </Typography>

      <Stack direction="row" spacing={2} justifyContent="center" mb={4}>
        <Button variant="contained" onClick={() => fetchAuditoria('criadas')}>
          Reservas Criadas
        </Button>
        <Button variant="contained" onClick={() => fetchAuditoria('excluidas')}>
          Reservas Excluídas
        </Button>
      </Stack>

      {erro && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {erro}
        </Alert>
      )}

      {carregando && (
        <Stack alignItems="center" mb={3}>
          <CircularProgress />
          <Typography mt={1}>Carregando...</Typography>
        </Stack>
      )}

      {!carregando && logs.length === 0 && tipoLog && (
        <Typography align="center" sx={{ mb: 3 }}>
          Nenhum log encontrado para "{tipoLog}".
        </Typography>
      )}

      {logs.length > 0 && (
        <Paper sx={{ p: 3 }}>
          <Typography variant="h6" gutterBottom>
            Logs de reservas {tipoLog}
          </Typography>
          <List>
            {logs.map((log) => (
              <div key={log.id}>
                <ListItem>
                  <ListItemText
                    primary={`Reserva ID: ${log.reservaId} - Usuário: ${log.usuarioEmail} (ID: ${log.usuarioId})`}
                    secondary={`Data e Hora: ${new Date(log.dataHoraAcao).toLocaleString()} - Ação: ${log.acao}`}
                  />
                </ListItem>
                <Divider component="li" />
              </div>
            ))}
          </List>
        </Paper>
      )}
    </Container>
  );
}
