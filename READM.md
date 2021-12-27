# Treasure Hunt

É um jogo desenvolvido em Unity 2020.3.20f1.

## Como jogar

- WASD ou setas para se movimentar
- Space para atirar as flechas

## Objetivo

Coletar a moeda que está em algum lugar do mapa e sair dele pela estrada que entrou.

## O que foi desenvolvido

### Player

- Player com movimentação 8D com animações que trocam corretamente pela direção que está indo
- Ataque em 4D com animações que trocam conforme o último movimento realizado
- Flecha que é disparada para a direção que está apontando
- Animação de flash quando o player toma um dano
- Corações que informam a vida do player no canto superior esquerdo
- Ao coletar a moeda, irá aparecer um ícone da moeda no canto superior direito da tela

### Inimigo:

- Possui uma range para perseguir o player
    - Quando o player sair da range, ele para de perseguir o player
    - Fica em idle apontando para a última direção
    - Troca de animações corretamente para a direção que está indo
- Ao colidir com o player, tira vida do player
- Animação flash quando toma algum dano
- Animação de morte para quando perder todas as vidas

### Moeda:

- Possui uma animação que vai trocando os sprites fazendo um efeito que está girando
- Possui uma animação que faz parecer um bouncing
- Ao ser coletada, desaparece e mostra um ícone no canto superior direito
- Só é permitido sair da tela se tiver coletado a moeda