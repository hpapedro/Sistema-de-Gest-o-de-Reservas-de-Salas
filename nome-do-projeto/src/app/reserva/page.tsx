'use client';

import { useEffect, useState } from 'react';
import {
  Button,
  Container,
  TextField,
  Typography,
  List,
  ListItem,
  ListItemText,
  Alert,
  Stack,
  Divider,
  Paper,
} from '@mui/material';

interface Reserva {
  id: number;
  salaId: number;
  dataHoraInicio: string;
  dataHoraFim: string;
}

const urlBase = 'http://localhost:5000/api';

export default function ReservasPage() {
  const [reservas, setReservas] = useState<Reserva[]>([]);
  const [erro, setErro] = useState('');
  const [salaId, setSalaId] = useState('');
  const [dataHoraInicio, setDataHoraInicio] = useState('');
  const [dataHoraFim, setDataHoraFim] = useState('');
  const [role, setRole] = useState('');

  useEffect(() => {
    // Carrega role do localStorage
    const userRole = localStorage.getItem('role') || '';
    setRole(userRole);

    // Carrega reservas conforme o role
    if (userRole.toLowerCase() === 'admin') {
      listarTodasReservas();
    } else {
      listarMinhasReservas();
    }
  }, []);

  // Função para fazer fetch autenticado
  async function fetchAutenticado(endpoint: string, method: string, body?: any) {
    const token = localStorage.getItem('token');
    if (!token) throw new Error('Token não encontrado. Faça login.');

    const options: RequestInit = {
      method,
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: body ? JSON.stringify(body) : undefined,
    };

    const resposta = await fetch(`${urlBase}${endpoint}`, options);

    if (!resposta.ok) {
      throw new Error(`Erro HTTP: ${resposta.status}`);
    }

    if (resposta.status === 204) return null;

    // Tenta adaptar resposta para um array de reservas
    const data = await resposta.json();
    if (Array.isArray(data)) return data;
    if (data && data.$values) return data.$values;
    return data;
  }

  async function listarMinhasReservas() {
    try {
      const dados = await fetchAutenticado('/reserva/minhas', 'GET');
      setReservas(dados);
      setErro('');
    } catch (e: any) {
      setErro(e.message);
      setReservas([]);
    }
  }

  async function listarTodasReservas() {
    try {
      const dados = await fetchAutenticado('/reserva', 'GET');
      setReservas(dados);
      setErro('');
    } catch (e: any) {
      setErro(e.message);
      setReservas([]);
    }
  }

  async function criarReserva() {
    if (!salaId || !dataHoraInicio || !dataHoraFim) {
      setErro('Preencha todos os campos para criar uma reserva.');
      return;
    }

    try {
      await fetchAutenticado('/reserva', 'POST', {
        salaId: Number(salaId),
        dataHoraInicio,
        dataHoraFim,
      });
      setErro('');
      if (role.toLowerCase() === 'admin') {
        listarTodasReservas();
      } else {
        listarMinhasReservas();
      }
      limparCampos();
    } catch (e: any) {
      setErro(e.message);
    }
  }

  async function deletarReserva(id: number) {
    try {
      await fetchAutenticado(`/reserva/remover/${id}`, 'DELETE');
      setErro('');
      if (role.toLowerCase() === 'admin') {
        listarTodasReservas();
      } else {
        listarMinhasReservas();
      }
    } catch (e: any) {
      setErro(e.message);
    }
  }

  function limparCampos() {
    setSalaId('');
    setDataHoraInicio('');
    setDataHoraFim('');
  }

  return (
    <Container maxWidth="md" sx={{ mt: 4 }}>
      <Typography variant="h4" gutterBottom align="center">
        Reservas ({role})
      </Typography>

      <Paper sx={{ p: 3, mb: 4 }} elevation={3}>
        <Typography variant="h6" gutterBottom>
          Nova Reserva
        </Typography>

        <Stack spacing={2} mb={2}>
          <TextField
            label="Sala ID"
            value={salaId}
            onChange={(e) => setSalaId(e.target.value)}
            fullWidth
            type="number"
          />

          <TextField
            label="Data e Hora Início"
            type="datetime-local"
            value={dataHoraInicio}
            onChange={(e) => setDataHoraInicio(e.target.value)}
            fullWidth
            InputLabelProps={{ shrink: true }}
          />

          <TextField
            label="Data e Hora Fim"
            type="datetime-local"
            value={dataHoraFim}
            onChange={(e) => setDataHoraFim(e.target.value)}
            fullWidth
            InputLabelProps={{ shrink: true }}
          />
        </Stack>

        <Button variant="contained" color="primary" onClick={criarReserva}>
          Criar Reserva
        </Button>

        <Stack direction="row" spacing={2} mt={2}>
          <Button variant="outlined" onClick={listarMinhasReservas}>
            Listar Minhas Reservas
          </Button>
          {role.toLowerCase() === 'admin' && (
            <Button variant="outlined" color="secondary" onClick={listarTodasReservas}>
              Listar Todas Reservas (Admin)
            </Button>
          )}
        </Stack>
      </Paper>

      {erro && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {erro}
        </Alert>
      )}

      {reservas.length > 0 ? (
        <Paper sx={{ p: 3 }}>
          <Typography variant="h6" gutterBottom>
            Reservas Encontradas
          </Typography>
          <List>
            {reservas.map((r) => (
              <div key={r.id}>
                <ListItem
                  secondaryAction={
                    role.toLowerCase() === 'admin' ? (
                      <Button
                        variant="outlined"
                        color="error"
                        onClick={() => deletarReserva(r.id)}
                      >
                        Remover
                      </Button>
                    ) : null
                  }
                >
                  <ListItemText
                    primary={`Reserva ID: ${r.id} - Sala ID: ${r.salaId}`}
                    secondary={`Início: ${new Date(r.dataHoraInicio).toLocaleString()} | Fim: ${new Date(r.dataHoraFim).toLocaleString()}`}
                  />
                </ListItem>
                <Divider component="li" />
              </div>
            ))}
          </List>
        </Paper>
      ) : (
        !erro && <Typography>Nenhuma reserva encontrada.</Typography>
      )}
    </Container>
  );
}
