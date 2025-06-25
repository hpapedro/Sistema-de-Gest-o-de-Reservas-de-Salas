'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import api from '@/services/api';
import { Sala } from '@/types/sala';

import {
  Box,
  Button,
  Container,
  TextField,
  Typography,
  Alert,
  Stack,
  Paper,
  List,
  ListItem,
  ListItemText,
  Divider,
} from '@mui/material';

export default function SalasPage() {
  const router = useRouter();
  const [salas, setSalas] = useState<Sala[]>([]);
  const [id, setId] = useState('');
  const [nome, setNome] = useState('');
  const [capacidade, setCapacidade] = useState('');
  const [salaUnica, setSalaUnica] = useState<Sala | null>(null);
  const [erro, setErro] = useState('');
  const [role, setRole] = useState('');

  useEffect(() => {
    const token = localStorage.getItem('token');
    const userRole = (localStorage.getItem('role') || '').toLowerCase();
    setRole(userRole);

    if (!token) {
      router.push('/login');
    }
    // Removido o auto listarSalas
  }, [router]);

  const listarSalas = async () => {
    try {
      const resposta = await fetch('http://localhost:5000/api/salas/listar');
      const dados = await resposta.json();
      setSalas(Array.isArray(dados) ? dados : dados.$values || []);
      setErro('');
      setSalaUnica(null);
    } catch {
      setErro('Erro ao listar salas');
      setSalas([]);
    }
  };

  const criarSala = async () => {
    try {
      await api.post('/salas/registrar', {
        nome,
        capacidade: Number(capacidade),
      });
      setErro('');
      listarSalas();
      limparCampos();
    } catch {
      setErro('Erro ao criar sala');
    }
  };

  const editarSala = async () => {
    try {
      await api.put(`/salas/editar/${id}`, {
        nome,
        capacidade: Number(capacidade),
      });
      setErro('');
      listarSalas();
      limparCampos();
    } catch {
      setErro('Erro ao editar sala');
    }
  };

  const buscarSala = async () => {
    try {
      const resposta = await api.get<Sala>(`/salas/${id}`);
      setSalaUnica(resposta.data);
      setErro('');
    } catch {
      setErro('Erro ao buscar sala');
      setSalaUnica(null);
    }
  };

  const deletarSala = async () => {
    try {
      await api.delete(`/salas/remover/${id}`);
      setErro('');
      listarSalas();
      limparCampos();
    } catch {
      setErro('Erro ao deletar sala');
    }
  };

  const limparCampos = () => {
    setId('');
    setNome('');
    setCapacidade('');
  };

  const podeEditar = role === 'admin';

  return (
    <Container maxWidth="md" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h3" gutterBottom align="center">
        Gerenciamento de Salas ({role || 'não autenticado'})
      </Typography>

      <Box sx={{ display: 'flex', justifyContent: 'center', mb: 3 }}>
        <Button variant="contained" color="primary" onClick={listarSalas}>
          Listar Salas
        </Button>
      </Box>

      {podeEditar && (
        <Paper sx={{ p: 3, mb: 4 }} elevation={3}>
          <Typography variant="h5" gutterBottom>
            Ações com JWT
          </Typography>
          <Stack spacing={2} direction={{ xs: 'column', sm: 'row' }} mb={2}>
            <TextField
              label="ID"
              variant="outlined"
              value={id}
              onChange={(e) => setId(e.target.value)}
              fullWidth
              type="number"
            />
            <TextField
              label="Nome"
              variant="outlined"
              value={nome}
              onChange={(e) => setNome(e.target.value)}
              fullWidth
            />
            <TextField
              label="Capacidade"
              variant="outlined"
              value={capacidade}
              onChange={(e) => setCapacidade(e.target.value)}
              fullWidth
              type="number"
            />
          </Stack>

          <Stack spacing={2} direction={{ xs: 'column', sm: 'row' }}>
            <Button variant="contained" color="success" onClick={criarSala} fullWidth>
              Criar Sala
            </Button>
            <Button variant="contained" color="warning" onClick={editarSala} fullWidth>
              Editar Sala
            </Button>
            <Button variant="contained" color="info" onClick={buscarSala} fullWidth>
              Buscar Sala por ID
            </Button>
            <Button variant="contained" color="error" onClick={deletarSala} fullWidth>
              Deletar Sala
            </Button>
          </Stack>
        </Paper>
      )}

      {erro && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {erro}
        </Alert>
      )}

      {salaUnica && (
        <Paper sx={{ p: 3, mb: 4 }} elevation={2}>
          <Typography variant="h6" gutterBottom>
            Resultado da Busca
          </Typography>
          <Typography>ID: {salaUnica.id}</Typography>
          <Typography>Nome: {salaUnica.nome}</Typography>
          <Typography>Capacidade: {salaUnica.capacidade}</Typography>
        </Paper>
      )}

      {salas.length > 0 && (
        <Paper sx={{ p: 3 }} elevation={2}>
          <Typography variant="h6" gutterBottom>
            Salas Listadas
          </Typography>
          <List>
            {salas.map((s) => (
              <div key={s.id}>
                <ListItem>
                  <ListItemText
                    primary={`ID: ${s.id} - ${s.nome}`}
                    secondary={`Capacidade: ${s.capacidade}`}
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
