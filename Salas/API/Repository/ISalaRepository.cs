using System;
using API.Models;
using System.Collections.Generic;

namespace API.Repository;

public interface ISalaRepository
{

    List<Sala> ListarTodas();
    Sala? BuscarPorId (int id);
    void Adicionar (Sala sala);
    void Atualizar(Sala sala);
    void Remover(int id);
    bool VerificarSeNomeExiste(string nome);
        bool VerificarSeNomeExisteComIdDiferente(string nome, int id);    
}