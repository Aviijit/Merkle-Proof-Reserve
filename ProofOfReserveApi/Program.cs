using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using MerkleLib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var users = new List<(int Id, int Balance)>
{
    (1,1111),
    (2,2222),
    (3,3333),
    (4,4444),
    (5,5555),
    (6,6666),
    (7,7777),
    (8,8888)
};

string SerializeUser((int Id, int Balance) user) => $"({user.Id},{user.Balance})";

string ComputeRoot() => MerkleTree.ComputeRootHex(users.Select(SerializeUser), "ProofOfReserve_Leaf", "ProofOfReserve_Branch");

app.MapGet("/root", () => new { root = ComputeRoot() });

app.MapGet("/proof/{id:int}", (int id) =>
{
    var index = users.FindIndex(u => u.Id == id);
    if (index == -1) return Results.NotFound();
    var values = users.Select(SerializeUser).ToList();
    var proof = MerkleTree.GetProof(values, index, "ProofOfReserve_Leaf", "ProofOfReserve_Branch");
    var proofStrings = proof.Select(p => $"({Convert.ToHexString(p.Hash).ToLowerInvariant()},{p.IsRight})").ToArray();
    return Results.Json(new { balance = users[index].Balance, path = proofStrings }, new JsonSerializerOptions { WriteIndented = true });
});

app.Run();
