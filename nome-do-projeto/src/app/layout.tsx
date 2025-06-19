import './globals.css';
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

import { Box, Container, CssBaseline, Typography } from '@mui/material';
import Header from '@/components/header'; // 👈 importa o novo cabeçalho

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="pt-br">
      <body>
        <CssBaseline />
        <Header /> {/* 👈 cabeçalho com botão "Home" */}
        <Box component="main" sx={{ minHeight: 'calc(100vh - 120px)', py: 4 }}>
          <Container>{children}</Container>
        </Box>
        <Box
          component="footer"
          sx={{
            bgcolor: '#1976d2',
            color: '#fff',
            py: 2,
            textAlign: 'center',
          }}
        >
          <Typography variant="body2">
            Desenvolvido por Pedro Henrique Picanço Alves
          </Typography>
        </Box>
      </body>
    </html>
  );
}
