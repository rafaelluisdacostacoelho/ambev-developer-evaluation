# 📌 Como Criar uma Tag para a main no GitHub

## 🏷️ 1. Crie uma Tag Localmente

Passos:

1. **Navegue até o repositório local:**

```bash
cd /caminho/para/seu/repositorio
```

2. **Certifique-se de estar na branch main:**

```bash
git checkout main
```

3. **Atualize a branch main:**

```bash
git pull origin main
```

4. **Crie a tag:**

```bash
git tag -a v1.0.0 -m "Minha primeira versão"
```

* `-a v1.0.0:` Cria uma tag anotada com o nome v1.0.0.

* `-m` "Minha primeira versão": Mensagem da tag.

5. **Envie a tag para o GitHub:**

```bash
git push origin v1.0.0
```

## ✨ 2. Alternativamente, Criar a Tag Diretamente pelo GitHub

1. Acesse o repositório no GitHub.

2. Vá para a aba Releases ou Tags.

3. Clique em Draft a new release.

4. Informe o nome da tag (ex.: v1.0.0).

5. Selecione a branch main para marcar.

6. Adicione um título e uma descrição.

7. Clique em Publish release.

## 💡 Dica: Verifique as Tags Existentes

```bash
git tag
```

Para mais detalhes de uma tag específica:

```bash
git show v1.0.0
```

## Conclusão
Sempre que for lançar uma versão, crie uma nova tag usando os passos anteriores, lembre-se apenas de mudar a versão com base no [Semantic Versioning](https://semver.org/) e Voilà! 😊

