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
  const [idParaDeletar, setIdParaDeletar] = useState('');
  const [role, setRole] = useState('');

  useEffect(() => {
    const userRole = localStorage.getItem('role') || '';
    setRole(userRole.toLowerCase());
  }, []);

async function fetchAutenticado(endpoint: string, method: string, body?: any) {
  const token = localStorage.getItem('token');
  if (!token) throw new Error('Token não encontrado. Faça login.');

  const headers: HeadersInit = {
    Authorization: `Bearer ${token}`,
  };

  if (body) {
    headers['Content-Type'] = 'application/json';
  }

  const options: RequestInit = {
    method,
    headers,
    body: body ? JSON.stringify(body) : undefined,
  };

  const resposta = await fetch(`${urlBase}${endpoint}`, options);
  const contentType = resposta.headers.get('content-type');

  if (!resposta.ok) {
    const text = await resposta.text();
    throw new Error(`Erro HTTP: ${resposta.status} - ${text}`);
  }

  if (resposta.status === 204 || !contentType?.includes('application/json')) return null;

  const data = await resposta.json();
  if (Array.isArray(data)) return data;
  if (data && data.$values) return data.$values;
  return data;
}


  async function listarMinhasReservas() {
    try {
      const dados = await fetchAutenticado('/reserva/minhas', 'GET');
      setReservas(dados || []);
      setErro('');
    } catch (e: any) {
      setErro(e.message);
      setReservas([]);
    }
  }

  async function listarTodasReservas() {
    try {
      const dados = await fetchAutenticado('/reserva', 'GET');
      setReservas(dados || []);
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
      limparCampos();
    } catch (e: any) {
      setErro(e.message);
    }
  }

  async function deletarReservaManual() {
    if (!idParaDeletar) {
      setErro('Informe o ID da reserva a ser deletada.');
      return;
    }

    try {
      await fetchAutenticado(`/reserva/remover/${idParaDeletar}`, 'DELETE');
      setErro('');
      setIdParaDeletar('');
      listarTodasReservas();
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

      {erro && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {erro}
        </Alert>
      )}

      {/* FORMULÁRIO SÓ PARA USUÁRIO */}
      {role !== 'admin' && (
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
        </Paper>
      )}

      {/* BOTÕES DE AÇÃO */}
      <Stack direction="row" spacing={2} mb={3}>
        {role !== 'admin' && (
          <Button variant="outlined" onClick={listarMinhasReservas}>
            Listar Minhas Reservas
          </Button>
        )}
        {role === 'admin' && (
          <Button variant="outlined" color="secondary" onClick={listarTodasReservas}>
            Listar Todas Reservas (Admin)
          </Button>
        )}
      </Stack>

      {/* CAMPO PARA DELETAR POR ID (apenas admin) */}
      {role === 'admin' && (
        <Paper sx={{ p: 3, mb: 4 }} elevation={2}>
          <Typography variant="h6" gutterBottom>
            Deletar Reserva (Admin)
          </Typography>
          <Stack direction="row" spacing={2}>
            <TextField
              label="ID da Reserva"
              type="number"
              value={idParaDeletar}
              onChange={(e) => setIdParaDeletar(e.target.value)}
              fullWidth
            />
            <Button variant="contained" color="error" onClick={deletarReservaManual}>
              Deletar
            </Button>
          </Stack>
        </Paper>
      )}

      {/* LISTA DE RESERVAS */}
      {reservas.length > 0 && (
        <Paper sx={{ p: 3 }}>
          <Typography variant="h6" gutterBottom>
            Reservas Encontradas
          </Typography>
          <List>
            {reservas.map((r) => (
              <div key={r.id}>
                <ListItem>
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
      )}
    </Container>
  );
}
