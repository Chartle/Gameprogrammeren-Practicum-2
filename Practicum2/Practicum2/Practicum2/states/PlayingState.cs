using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practicum2.states
{
    abstract class PlayingState : GameObjectList
    {
        // Used as a template for different playing states
        public abstract int GetScore();
        public abstract void SetScore(int score);
    }
}
