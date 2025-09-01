using System;
using UnityEngine;

// Every method of this class without parameters and that returns a bool value 
// can be used to trigger the reasoner.
namespace ThinkEngine
{
    // Definizione di un ScriptableObject che può essere usato per i trigger
    public class ThinkEngineTrigger : ScriptableObject
    {
        private float accumulatedTime = 0f; // Variabile che accumula il tempo trascorso tra i trigger
        private int lastFrame = -1;         // Tiene traccia del frame precedente per evitare di accumulare deltaTime più volte nello stesso frame
        private const float interval = 2f;  // Intervallo minimo in secondi tra due trigger consecutivi

        public bool EnemyTrigger()
        {
            // Controlla se siamo in un nuovo frame rispetto all'ultimo in cui è stato chiamato il trigger
            if (Time.frameCount != lastFrame)
            {
                // Se il tempo accumulato supera già l'intervallo, lo resettiamo
                // Questo evita di accumulare troppa differenza di tempo tra frame distanti
                if (accumulatedTime > interval)
                    accumulatedTime = 0;

                // Aggiorna l'ultimo frame processato
                lastFrame = Time.frameCount;

                // Accumula il tempo trascorso in questo frame
                accumulatedTime += Time.deltaTime;
            }

            // Se il tempo accumulato ha raggiunto o superato l'intervallo desiderato,
            // il trigger scatta e ritorna true
            if (accumulatedTime >= interval)
            {
                return true;
            }

            // Altrimenti non è ancora tempo, ritorna false
            return false;
        }
    }
}
