using Yarn.Unity;

namespace Game.Yarn
{
    public class NormalDialogueView : DialogueViewBase
    {
        public LineView view;
        
        public void Interrupt()
        {
            view.currentStopToken.Interrupt();
        }
    }
}