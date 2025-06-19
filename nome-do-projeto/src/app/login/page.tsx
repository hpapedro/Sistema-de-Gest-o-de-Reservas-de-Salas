'use client';

import React, { useState } from 'react';
import { useRouter } from 'next/navigation';
import { TextField, Button, Typography, Container, Alert, Stack, Paper } from '@mui/material';

interface LoginResponse {
  token: string;
  role?: string;
}

async function login(email: string, senha: string): Promise<LoginResponse> {
  const resposta = await fetch('http://localhost:5000/api/auth/login', {
    method: 'POST',
    body: JSON.stringify({
      Email: email,
      Senha: senha,
    }),
    headers: {
      'Content-Type': 'application/json',
    },
  });

  if (!resposta.ok) {
    throw new Error('Erro ao fazer login');
  }

  return resposta.json();
}

export default function LoginPage() {
  const router = useRouter();

  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [erro, setErro] = useState('');

  const handleLogin = async () => {
    setErro('');
    try {
      const { token, role } = await login(email, senha);
      localStorage.setItem('token', token);
      localStorage.setItem('role', role ?? 'user');
      router.push('/dashboard');
    } catch (err: any) {
      setErro(err.message);
    }
  };

  return (
    <Container maxWidth="sm" sx={{ mt: 8 }}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h4" gutterBottom align="center">
          Login
        </Typography>

        <Stack spacing={3}>
          <TextField
            label="E-mail"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            fullWidth
            required
          />

          <TextField
            label="Senha"
            type="password"
            value={senha}
            onChange={(e) => setSenha(e.target.value)}
            fullWidth
            required
          />

          {erro && <Alert severity="error">{erro}</Alert>}

          <Button variant="contained" onClick={handleLogin} fullWidth>
            Entrar
          </Button>
        </Stack>
      </Paper>
    </Container>
  );
}
