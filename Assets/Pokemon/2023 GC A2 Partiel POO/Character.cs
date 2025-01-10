using System;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition d'un personnage
    /// </summary>
    public class Character
    {
        /// <summary>
        /// Stat de base, HP
        /// </summary>
        int _baseHealth;
        /// <summary>
        /// Stat de base, ATK
        /// </summary>
        int _baseAttack;
        /// <summary>
        /// Stat de base, DEF
        /// </summary>
        int _baseDefense;
        /// <summary>
        /// Stat de base, SPE
        /// </summary>
        int _baseSpeed;
        /// <summary>
        /// Type de base
        /// </summary>
        TYPE _baseType;

        public Character(int baseHealth, int baseAttack, int baseDefense, int baseSpeed, TYPE baseType)
        {
            _baseHealth = baseHealth;
            _baseAttack = baseAttack;
            _baseDefense = baseDefense;
            _baseSpeed = baseSpeed;
            _baseType = baseType;
            CurrentHealth = _baseHealth;
        }
        /// <summary>
        /// HP actuel du personnage
        /// </summary>
        private int _currentHealth;
        public int CurrentHealth 
        {
            get
            {
                CurrentHealth = _currentHealth;
                return _currentHealth; 
            } 
            private set
            {
                _currentHealth = value; 
                if (_currentHealth < 0) _currentHealth = 0;
                if (_currentHealth > MaxHealth) _currentHealth = MaxHealth;
            } 
        }

        public TYPE BaseType { get => _baseType;}
        /// <summary>
        /// HPMax, prendre en compte base et equipement potentiel
        /// </summary>
        public int MaxHealth
        {
            get
            {
                if (CurrentEquipment != null) return _baseHealth + CurrentEquipment.BonusHealth;
                return _baseHealth;
            }
        }
        /// <summary>
        /// ATK, prendre en compte base et equipement potentiel
        /// </summary>
        public int Attack
        {
            get
            {
                if(CurrentEquipment != null) return _baseAttack + CurrentEquipment.BonusAttack;
                return _baseAttack;
            }
        }
        /// <summary>
        /// DEF, prendre en compte base et equipement potentiel
        /// </summary>
        public int Defense
        {
            get
            {
                if (CurrentEquipment != null) return _baseDefense + CurrentEquipment.BonusDefense;
                return _baseDefense;
            }
        }
        /// <summary>
        /// SPE, prendre en compte base et equipement potentiel
        /// </summary>
        public int Speed
        {
            get
            {
                if (CurrentEquipment != null) return _baseSpeed + CurrentEquipment.BonusSpeed;
                return _baseSpeed;
            }
        }
        /// <summary>
        /// Equipement unique du personnage
        /// </summary>
        public Equipment CurrentEquipment { get; private set; }
        /// <summary>
        /// null si pas de status
        /// </summary>
        public StatusEffect CurrentStatus { get; private set; }

        public bool IsAlive => CurrentHealth > 0;

        /// <summary>
        /// Application d'un skill contre le personnage
        /// On pourrait potentiellement avoir besoin de connaitre le personnage attaquant,
        /// Vous pouvez adapter au besoin
        /// </summary>
        /// <param name="s">skill attaquant</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ReceiveAttack(Skill s)
        {
            if(s.CurrentPP == 0) return;
            ReceiveDamage((int)(s.Power * TypeResolver.GetFactor(s.Type, BaseType)) - Defense);
            if (s.Status != StatusPotential.NONE) CurrentStatus = StatusEffect.GetNewStatusEffect(s.Status);
            s.SkillUsed();
        }
        /// <summary>
        /// Application de dégat
        /// </summary>
        /// <param name="s">valeur des dégats</param>
        public void ReceiveDamage(int damage)
        {
            CurrentHealth -= damage;
        }
        /// <summary>
        /// Application d'un un antidote supprimant les effets de statut
        /// </summary>
        /// <param name="s">valeur du soin</param>
        public void Antidote()
        {
            CurrentStatus = StatusEffect.GetNewStatusEffect(StatusPotential.NONE);
        }
        /// <summary>
        /// Application d'un heal contre le personnage
        /// </summary>
        /// <param name="s">valeur du soin</param>
        public void ReceiveHeal(int heal)
        {
            CurrentHealth += heal;
        }
        /// <summary>
        /// Equipe un objet au personnage
        /// </summary>
        /// <param name="newEquipment">equipement a appliquer</param>
        /// <exception cref="ArgumentNullException">Si equipement est null</exception>
        public void Equip(Equipment newEquipment)
        {
            if(newEquipment == null) throw new ArgumentNullException();
            else CurrentEquipment = newEquipment;
        }
        /// <summary>
        /// Desequipe l'objet en cours au personnage
        /// </summary>
        public void Unequip()
        {
            CurrentEquipment = null;
        }

        public bool HasPriorityEquipement => CurrentEquipment != null && CurrentEquipment.BonusPriority;
        public bool CanAttack => CurrentStatus == null || CurrentStatus.CanAttack;
    }
}
