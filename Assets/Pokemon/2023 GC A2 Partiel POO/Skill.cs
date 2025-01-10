
namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition abstraite d'une compétence.
    /// </summary>
    public abstract class Skill
    {
        public Skill(TYPE type, int power, int Pp, StatusPotential status)
        {
            Type = type;
            Power = power;
            PP = Pp;
            Status = status;
            CurrentPP = PP;
        }

        /// <summary>
        /// Le type de l'attaque, à prendre en compte lors de la résolution des résistance/faiblesses
        /// </summary>
        public TYPE Type { get; private set; }
        /// <summary>
        /// La puissance du coup, à prendre en compte lors de la résolution des dégâts
        /// </summary>
        public int Power { get; private set; }
        /// <summary>
        /// Le nombre de fois que la compétence peut etre utiliser
        /// </summary>
        public int PP { get; private set; }
        public int CurrentPP { get; private set; }
        /// <summary>
        /// Le statut infligé à la cible à la suite de l'attaque
        /// </summary>
        public StatusPotential Status { get; private set; }

        public virtual void SkillUsed() 
        {
            CurrentPP--;
            if(CurrentPP < 0) CurrentPP = 0;
        }
    }

}
