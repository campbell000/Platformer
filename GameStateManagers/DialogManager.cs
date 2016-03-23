using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlatformerGame.GameObjects;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables;
using PlatformerGame.GameStateManagers;
using Microsoft.Xna.Framework.Graphics;
using PlatformerGame.GameStates;

namespace PlatformerGame.GameStateManagers
{
    /**
     * This class is responsible for "tidying" up text so that it appears in the dialog box accurately. It spaces out and breaks
     * up lines of text so that the lines appear correctly and fully in the dialog box
     **/
    public class DialogManager : StateManager
    {
        /* Constants */
        private const int MAX_LINE_WIDTH = 1770;
        public const float CHARS_PER_MILLI = .1f;
        public static Vector2 DIALOG_SIZE = new Vector2(1920, 300);
        private const int NUM_LINES_DIALOG = 2;
        public static Vector2 LINE_ONE_POSITION = 
            new Vector2(50, Resolution.getInternalHeight() - DIALOG_SIZE.Y + 20);

        //State variables
        public bool readyToInteract { get; set; }
        private bool doneDisplaying = false;

        public String rawDialog;
        public Interactable currentNPC { get; set; }
        public SpriteFont font;
        
        //Variables related to the current text/character positions in a frame
        List<Int32> lineBreakPositions;
        private int currentCharPointer = 0;
        StringBuilder currentText;
        private int currentLineNum = 0;
        private int linesInDialog = 0;
        private int linesPassed = 0;
        

        public DialogManager(GameObjectContainer container, SpriteFont font)
            : base(container)
        {
            readyToInteract = true;
            currentNPC = null;
            this.font = font;
            this.thisState = GameState.DIALOG;
            this.lineBreakPositions = new List<Int32>();
        }

        public override void prepareToEnterState(GameObject objectToPass)
        {
            enterDialogState((Interactable)objectToPass);
        }

        /**
         * This method should be called to enter a dialog state with an NPC
         **/
        private void enterDialogState(Interactable npc)
        {
            resetVars();
            gameObjects.addHUDObject(gameObjects.dialogBox);

            rawDialog = npc.dialog;
            preprocessLinebreaks();
            currentText = new StringBuilder();
        }

        /**
         * This method inserts line breaks in an NPC's text so that the words properly wrap
         * around the dialog box
         */
        private void preprocessLinebreaks()
        {
            StringBuilder str = new StringBuilder();
            int currentLength = 0;
            for (int i = 0; i < rawDialog.Length; i++)
            {
                str.Append(rawDialog[i]);
                currentLength = (int)font.MeasureString(str).X;
                if (currentLength > MAX_LINE_WIDTH)
                {
                    str = insertNewLineBreak(str);
                    linesInDialog++;
                }
            }
            rawDialog = str.ToString();
        }

        /**
         * This method inserts a line break into the text. Specifically,
         * it looks for the right-most space, and replaces it with a new line
         **/
        private StringBuilder insertNewLineBreak(StringBuilder str)
        {
            for (int j = str.Length - 1; j >= 0; j--)
            {
                if (str[j] == ' ')
                {
                    str[j] = '\n';
                    break;
                }
            }
            return str;
        }

        private void resetVars()
        {
            currentCharPointer = 0;
            this.stateToEnter = GameState.PLAY;
            this.isExitingState = false;
            readyToInteract = false;
        }

        public override void updateState(GameTime delta, InputState inputState)
        {
            if (inputState.interactWasReleased())
            {
                updateDialog(delta);
                readyToInteract = true;
            }
            else if (readyToInteract && inputState.interactWasPressed())
            {
                actionWasPressed();
                readyToInteract = false;
            }
        }

        /**
         * This method updates the dialog box baed on the amount of time that has passed since
         * the last frame.
         */
        private void updateDialog(GameTime delta)
        {
            int prevCharPointer = currentCharPointer;
            double millisSinceLastUpdate = delta.ElapsedGameTime.TotalMilliseconds;

            if (currentLineNum < NUM_LINES_DIALOG)
            {
                currentCharPointer += (int)(millisSinceLastUpdate * CHARS_PER_MILLI);
                if (currentCharPointer > rawDialog.Length)
                    currentCharPointer = rawDialog.Length;

                addCharsToTextBox(prevCharPointer, currentCharPointer);
            }
            else
                doneDisplaying = true;
        }

        private void addCharsToTextBox(int previousPointer, int currentPointer)
        {
            for (int i = previousPointer; i < currentCharPointer; i++)
            {
                currentText.Append(rawDialog[i]);
                if (rawDialog[i] == '\n')
                {
                    currentLineNum++;
                    linesPassed++;
                }
            }
        }

        private void actionWasPressed()
        {
            //If there are no more lines and the current character pointer is at the end, close it
            if (linesPassed >= linesInDialog && currentCharPointer >= (rawDialog.Length - 1))
                closeOutDialog();
            else
            {
                if (doneDisplaying)
                {
                    currentLineNum = 0;
                    currentText.Clear();
                    doneDisplaying = false;
                }
                else
                {
                    fastForwardText();
                }
            }
        }

        private void fastForwardText()
        {
            int currentLineDisplayed = currentLineNum + 1;
            int previous = currentCharPointer;
            int i;
            
            //Iterate through more text until we are currently displaying the max amount of lines.
            for (i = currentCharPointer; i < rawDialog.Length; i++)
            {
                if (rawDialog[i] == '\n')
                {
                    if (currentLineDisplayed >= NUM_LINES_DIALOG)
                        break;
                    else
                        currentLineDisplayed++;
                }
            }

            //Use i, which should point to the last char in the text box, to advance the text.
            currentCharPointer = i;
            addCharsToTextBox(previous, currentCharPointer);
            doneDisplaying = true;
        }

        public StringBuilder getDialogText()
        {
            return currentText;
        }

        public void closeOutDialog()
        {
            resetStateVariables();
            gameObjects.HUDObjects.Remove(gameObjects.dialogBox);
            this.stateToEnter = GameStates.GameState.PLAY;
        }

        private void resetStateVariables()
        {
            this.currentText.Clear();
            currentLineNum = 0;
            currentCharPointer = 0;
            this.doneDisplaying = false;
            this.isExitingState = true;
        }

        public override void drawStateSpecificObjects(GameTime t, SpriteBatch batch)
        {
            Drawer.drawText(font, t, batch, getDialogText(), 80, (int)Resolution.getInternalHeight() - 220);
        }

        public override GameStates.GameState getManagerType()
        {
            return GameStates.GameState.PLAY;
        }
    }
}
