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
    public class old_dialog : StateManager
    {
        public bool finishedDialog { get; set; }
        public bool readyToInteract { get; set; }
        public String rawDialog;
        public Interactable currentNPC { get; set; }
        public bool isCurrentlyDisplayingDialog { get; set; }
        public SpriteFont font;
        public const float CHARS_PER_MILLI = .1f;
        private int currentCharPointer = 0;
        private const int MAX_LINE_WIDTH = 1770;

        public static Vector2 DIALOG_SIZE = new Vector2(1920, 300);
        public static Vector2 LINE_ONE_POSITION = new Vector2(50, Resolution.getInternalHeight() - DIALOG_SIZE.Y + 20);

        private const int NUM_LINES_DIALOG = 2;
        private StringBuilder[] currentTextLines;
        private int currentLineNum = 0;

        public old_dialog(GameObjectContainer container, SpriteFont font)
            : base(container)
        {
            finishedDialog = false;
            readyToInteract = true;
            currentNPC = null;
            isCurrentlyDisplayingDialog = false;
            this.font = font;
            this.thisState = GameState.DIALOG;
        }

        private void initCurrentTextLines()
        {
            currentTextLines = new StringBuilder[NUM_LINES_DIALOG];
            for (int i = 0; i < NUM_LINES_DIALOG; i++)
            {
                currentTextLines[i] = new StringBuilder();
            }
        }

        public override void prepareToEnterState(GameObject objectToPass)
        {
            enterDialogState((Interactable)objectToPass);
        }

        private void enterDialogState(Interactable npc)
        {
            resetVars();
            gameObjects.addHUDObject(gameObjects.dialogBox);

            rawDialog = npc.dialog;
            initCurrentTextLines();
        }

        private void resetVars()
        {
            currentCharPointer = 0;
            currentLineNum = 0;
            this.stateToEnter = GameState.PLAY;
            this.isExitingState = false;
            readyToInteract = false;
            isCurrentlyDisplayingDialog = true;
            finishedDialog = false;
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
                advanceDialog();
            }
        }

        private void updateDialog(GameTime delta)
        {
            int prevCharPointer = currentCharPointer;
            double millisSinceLastUpdate = delta.ElapsedGameTime.TotalMilliseconds;

            currentCharPointer += (int)(millisSinceLastUpdate * CHARS_PER_MILLI);
            if (currentCharPointer > rawDialog.Length)
                currentCharPointer = rawDialog.Length;

            addCharsToTextBox(prevCharPointer, currentCharPointer);
        }

        private void addCharsToTextBox(int previousPointer, int currentPointer)
        {
            for (int i = previousPointer; i < currentCharPointer; i++)
            {
                int currentLength = (int)font.MeasureString(this.currentTextLines[currentLineNum]).X;
                int potentialLength = currentLength + (int)font.MeasureString(rawDialog[i].ToString()).X;

                if (previousPointer < rawDialog.Length)
                {
                    if (potentialLength > MAX_LINE_WIDTH)
                        doWordWrap();

                    this.currentTextLines[currentLineNum].Append(rawDialog[i]);
                }
            }
        }

        private void doWordWrap()
        {
            //Traverse the current line to find the LAST space. 
            int indexOfLastSpace = -1;
            for (int i = 0; i < this.currentTextLines[currentLineNum].Length; i++)
            {
                StringBuilder currentTextLine = this.currentTextLines[currentLineNum];
                char c = currentTextLine[i];
                if (c == ' ')
                    indexOfLastSpace = i;
            }

            if (indexOfLastSpace != -1)
                this.currentTextLines[currentLineNum][indexOfLastSpace] = '\n';
            else
                this.currentTextLines[currentLineNum].Append("\n");

            currentLineNum++;
        }

        private void advanceDialog()
        {
            if (true)
                closeOutDialog();
        }

        public String getDialogText()
        {
            StringBuilder allText = new StringBuilder();
            for (int i = 0; i < NUM_LINES_DIALOG; i++)
            {
                allText.Append(currentTextLines[i]);
            }
            return allText.ToString();
        }

        public void closeOutDialog()
        {
            isCurrentlyDisplayingDialog = false;
            gameObjects.HUDObjects.Remove(gameObjects.dialogBox);

            this.stateToEnter = GameStates.GameState.PLAY;
            this.isExitingState = true;
        }

        public override void drawStateSpecificObjects(GameTime t, SpriteBatch batch)
        {
            if (isCurrentlyDisplayingDialog)
                Drawer.drawText(font, t, batch, getDialogText(), 80, (int)Resolution.getInternalHeight() - 220);
        }

        public override GameStates.GameState getManagerType()
        {
            return GameStates.GameState.PLAY;
        }
    }
}
