
using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer des features et les TU sur le reste du projet

        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
            // - un heal ne régénère pas plus que les HP Max

        [Test]
        public void CharacterReceiveHeal()
        {
            var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            var punch = new Punch();
            var oldHealth = pikachu.CurrentHealth;
            int heal = 30;

            pikachu.ReceiveAttack(punch); // hp : 100 => 60
            pikachu.ReceiveHeal(heal); // hp : 60 => 90
            Assert.That(pikachu.CurrentHealth,
                Is.EqualTo(oldHealth - (punch.Power - pikachu.Defense) + heal)); // 100 - (70-30) + 30
            pikachu.ReceiveHeal(heal); // hp : 90 => 100
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(pikachu.MaxHealth));
        }
            // - si on abaisse les HPMax les HP courant doivent suivre si c'est au dessus de la nouvelle valeur
        [Test]
        public void CharacterHPmaxChange()
        {
            var pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            var LifeFruit = new Equipment(10, 0, 0, 0);

            Assert.That(pikachu.CurrentHealth, Is.EqualTo(pikachu.MaxHealth)); // MaxHP : 100, hp : 100
            pikachu.Equip(LifeFruit);// MaxHP : 100 => 110
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(pikachu.MaxHealth - 10));// MaxHP : 110, hp : 100
            pikachu.ReceiveHeal(10); // hp : 100 => 110
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(pikachu.MaxHealth));// MaxHP : 110, hp : 110
            pikachu.Unequip();
            Assert.That(pikachu.CurrentHealth, Is.EqualTo(pikachu.MaxHealth));// MaxHP : 100, hp : 100
        }
            // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        [Test]
        public void HasPriorityWithPriorityEquipement()
        {
            Character mew = new Character(10, 5000, 0, 20, TYPE.NORMAL);
            Character mewtwo = new Character(10, 5000, 0, 200, TYPE.NORMAL);
            Fight f = new Fight(mew, mewtwo);
            MegaPunch mp = new MegaPunch();

            // Both can OneShot but mew is slower
            f.ExecuteTurn(mp, mp);

            Assert.That(mew.IsAlive, Is.EqualTo(false));
            Assert.That(mewtwo.IsAlive, Is.EqualTo(true));

            mew.ReceiveHeal(100);
            mewtwo.ReceiveHeal(100);
            var Flash = new Equipment(10, 0, 0, 0, true);
            mew.Equip(Flash);

            // Both can OneShot but mew as a Priority equipement
            f.ExecuteTurn(mp, mp);

            Assert.That(mew.IsAlive, Is.EqualTo(true));
            Assert.That(mewtwo.IsAlive, Is.EqualTo(false));
        }
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
        [Test]
        public void StatutSleep()
        {
            Character pikachu = new Character(2000, 50, 30, 20, TYPE.NORMAL);
            Character bulbizarre = new Character(90, 60, 10, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, bulbizarre);
            Punch p = new Punch();
            MagicalGrass mg = new MagicalGrass();
            Assert.That(mg.Status, Is.EqualTo(StatusPotential.SLEEP));

            Assert.That(bulbizarre.CurrentStatus, Is.EqualTo(null));
            // Bulbizarre endort Pikachu
            f.ExecuteTurn(p, mg);
            Assert.That(bulbizarre.CurrentHealth, Is.EqualTo(bulbizarre.MaxHealth));
            // Pikachu dort encore 4 tours
            Assert.That(pikachu.CurrentStatus.RemainingTurn, Is.EqualTo(4));
            f.ExecuteTurn(p, p);
            // Pikachu dort encore 3 tours
            f.ExecuteTurn(p, p);
            // Pikachu dort encore 2 tours
            f.ExecuteTurn(p, p);
            // Pikachu dort encore 1 tours
            Assert.That(pikachu.CurrentStatus.RemainingTurn, Is.EqualTo(1));
            f.ExecuteTurn(p, p);
            // Pikachu se reveille
            f.ExecuteTurn(p, p);
            Assert.That(bulbizarre.CurrentHealth, Is.LessThan(bulbizarre.MaxHealth));
        }
        [Test]
        public void StatutBurn()
        {
            Character pikachu = new Character(2000, 50, 30, 20, TYPE.NORMAL);
            Character salameche = new Character(90, 60, 10, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, salameche);
            Punch p = new Punch();
            FireBall fb = new FireBall();
            Assert.That(fb.Status, Is.EqualTo(StatusPotential.BURN));

            Assert.That(pikachu.CurrentStatus, Is.EqualTo(null));
            // salameche brule Pikachu
            f.ExecuteTurn(p, fb);
            Assert.That(pikachu.CurrentHealth, Is.LessThan(pikachu.MaxHealth - (fb.Power - pikachu.Defense)));
        }
        [Test]
        public void Antidote()
        {
            Character pikachu = new Character(2000, 50, 30, 20, TYPE.NORMAL);
            Character bulbizarre = new Character(90, 60, 10, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, bulbizarre);
            Punch p = new Punch();
            MagicalGrass mg = new MagicalGrass();
            Assert.That(mg.Status, Is.EqualTo(StatusPotential.SLEEP));

            Assert.That(bulbizarre.CurrentStatus, Is.EqualTo(null));
            // Bulbizarre endort Pikachu
            f.ExecuteTurn(p, mg);
            Assert.That(bulbizarre.CurrentHealth, Is.EqualTo(bulbizarre.MaxHealth));
            Assert.That(pikachu.CurrentStatus.RemainingTurn, Is.EqualTo(4));
            pikachu.Antidote();
            Assert.That(bulbizarre.CurrentStatus, Is.EqualTo(null));
        }
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
        [Test]
        public void ForceFaiblesse()
        {
            Character pikachu = new Character(2000, 50, 30, 200, TYPE.NORMAL);
            Character bulbizarre = new Character(90, 60, 10, 20, TYPE.GRASS);
            Character salameche = new Character(90, 60, 10, 20, TYPE.FIRE);
            Fight f = new Fight(pikachu, salameche);
            Punch p = new Punch();
            WaterBlouBlou wbb = new WaterBlouBlou();

            
            f.ExecuteTurn(wbb, p);
            Assert.That(salameche.CurrentHealth, Is.LessThan(salameche.MaxHealth - (wbb.Power - salameche.Defense)));

            f = new Fight(pikachu, bulbizarre);
            f.ExecuteTurn(wbb, p);
            Assert.That(bulbizarre.CurrentHealth, Is.GreaterThan(bulbizarre.MaxHealth - (wbb.Power - bulbizarre.Defense)));
        }
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type
        // - L'utilisation d'objets : Potion, SuperPotion, Vitess+, Attack+ etc.
        // - Gérer les PP (limite du nombre d'utilisation) d'une attaque,
        // si on selectionne une attack qui a 0 PP on inflige 0

        // Choisis ce que tu veux ajouter comme feature et fait en au max.
        // Les nouveaux TU doivent être dans ce fichier.
        // Modifiant des features il est possible que certaines valeurs
        // des TU précédentes ne matchent plus, tu as le droit de réadapter les valeurs
        // de ces anciens TU pour ta nouvelle situation.

    }
}
