using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace PackOpeningTracker
{
    public class StatisticsData
    {
        private List<Card> CardDatabase;
        private List<Card> CardsFromPacks;
        private List<Card> CardsFromPacksInOrder;
        private List<int> DustPerCard;
        private int noSLegendaries, noSEpics, noSRares, noSCommons, noSTotal,
                    noGLegendaries, noGEpics, noGRares, noGCommons, noGTotal,
                    noELegendaries, noEEpics, noERares, noECommons, noETotal,
                    noALegendaries, noAEpics, noARares, noACommons, noATotal,
                    totalDust, highestDustPack, no40DustPacks, totalPacks,extraCardsDust;
        private float avgDustPerPack;
        private List<Card> bestPack;
        private Controller ctrl;
        private List<int> classicPositions, gvgPositions, tgtPositions;

        public StatisticsData(Controller controller)
        {
            CardDatabase = new List<Card>();
            CardsFromPacks = new List<Card>();
            CardsFromPacksInOrder = new List<Card>();
            DustPerCard = new List<int>();
            classicPositions = new List<int>();
            gvgPositions = new List<int>();
            tgtPositions = new List<int>();
            noSCommons = 0;
            noSRares = 0;
            noSEpics = 0;
            noSLegendaries = 0;
            noSTotal = 0;
            noGCommons = 0;
            noGRares = 0;
            noGEpics = 0;
            noGLegendaries = 0;
            noGTotal = 0;
            noECommons = 0;
            noERares = 0;
            noEEpics = 0;
            noELegendaries = 0;
            noETotal = 0;
            noACommons = 0;
            noARares = 0;
            noAEpics = 0;
            noALegendaries = 0;
            noATotal = 0;
            extraCardsDust = 0;
            avgDustPerPack = 0;
            totalDust = 0;
            highestDustPack = 0;
            no40DustPacks = 0;
            totalPacks = 0;
            bestPack = new List<Card>();
            ctrl = controller;

            if (File.Exists(@".\Data\CardDatabase.dat"))
            {
                string file = File.ReadAllText(@".\Data\CardDatabase.dat");
                string[] lines = file.Split('\n');

                foreach (string line in lines)
                {
                    string aux = line.Replace("\r", string.Empty);
                    string[] details = aux.Split('|');
                    Card card = new Card(details[0], details[1], details[2], details[3]);
                    CardDatabase.Add(card);
                }
            }
            else
                MessageBox.Show("Unable to load 'CardDatabase.dat' because the file appears to be missing. Pack Tracker will not be able to track without that file. Please redownload Pack Tracker or provide 'CardDatabase.dat'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public List<Card> cfp
        {
            get
            {
                return CardsFromPacks;
            }
            set
            {
                CardsFromPacks = value;
            }
        }

        public List<Card> cfpInOrder
        {
            get
            {
                return CardsFromPacksInOrder;
            }
        }

        public List<int> ClassicPositions
        {
            get
            {
                return classicPositions;
            }
        }

        public List<int> GVGPositions
        {
            get
            {
                return gvgPositions;
            }
        }

        public List<int> TGTPositions
        {
            get
            {
                return tgtPositions;
            }
        }

        public List<int> Stats
        {
            get
            {
                return new List<int>() { noSLegendaries, noSEpics, noSRares, noSCommons, noSTotal,
                                         noGLegendaries, noGEpics, noGRares, noGCommons, noGTotal,
                                         noELegendaries, noEEpics, noERares, noECommons, noETotal,
                                         noALegendaries, noAEpics, noARares, noACommons, noATotal,
                                         totalDust, highestDustPack, no40DustPacks, totalPacks,extraCardsDust };
            }
        }

        public float AvgDustPerPack
        {
            get
            {
                return avgDustPerPack;
            }
        }

        public List<Card> BestPack
        {
            get
            {
                return bestPack;
            }
        }

        public void collectData()
        {
            ResetStats();
            cardCounter();
            dustCounter();
            calculateIndividualPacksPosition();
        }

        public void cardDigger()
        {
            try
            {
                ResetCards();

                using (FileStream stream = File.Open(ctrl.getHSPath() + @"\Logs\Achievements.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string text = reader.ReadToEnd();   // read the log

                        string[] lines = text.Split('\n');      // split it in lines

                        foreach (string line in lines)  // check each line
                        {
                            string[] words = line.Split(' ');   // we split each line by ' ' (empty space)

                            int limit1 = -1, limit2 = -1;
                            for (int i = 0; i < words.Count(); i++)
                            {
                                if (words[i] == "NotifyOfCardGained:")  // we mark the position after which the name of the card starts (if any) 
                                    limit1 = i;
                                else
                                    if (words[i].Contains("cardId"))    // we mark the position before which the name of the card ends
                                        limit2 = i;
                            }                                           // we do the above because the name might be composed of multiple words

                            if (limit1 != -1)   // if we have found a card on the current line we extract its name and type
                            {
                                string cardName = "";
                                string premium = "";

                                for (int i = 0; i < words.Count(); i++)
                                {
                                    if (i > limit1 && i < limit2)
                                    {
                                        if (words[i].Contains("[name="))
                                            words[i] = words[i].Substring(6);
                                        cardName += words[i] + " ";
                                    }
                                    else
                                        if (i > limit2)
                                            if (words[i].Contains("Premium="))
                                            {
                                                premium = words[i].Substring(8);
                                                premium = premium.Remove(premium.Length - 1);
                                            }
                                }

                                cardName = cardName.Substring(0, cardName.Length - 1);

                                // here we already know the name of the card and its type (Golden/NonGolden)

                                addNewCard(cardName, premium);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StreamWriter file = new StreamWriter(DateTime.Now.ToString("yyyy-dd-MM--HH-mm-ss") + "_Crash_Log.txt");
                file.Write(ex.ToString());
                file.Close();
                MessageBox.Show("Unhandled exception found. A log file containing the details of the exception has been saved to parent directory. If possible, send the log to the owner of this product using one of the means displayed in 'About'. Sorry for any inconveniences caused by this error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void addNewCard(string cardName, string premium)
        {
            int position = getCardPosition(cardName);   // we search for the card in the the card database
            
            if (position != -1) // if the card is not basic we create a new card and save it in CardsFromPacks (along with all its details)
            {
                int auxPos = getCardPosition(cardName,  premium); // we look to see if we have already opened this card

                if (auxPos != -1)   // if yes, then we raise its count
                    CardsFromPacks[auxPos].cardCount++;
                else                // if not, then we add it
                    CardsFromPacks.Add(new Card(cardName, CardDatabase[position].cardRarity, CardDatabase[position].cardClass, CardDatabase[position].cardSet, premium));

                // we also add the card into another list with each individual card opened in order (including duplicates)
                CardsFromPacksInOrder.Add(new Card(cardName, CardDatabase[position].cardRarity, CardDatabase[position].cardClass, CardDatabase[position].cardSet, premium));

                switch (CardDatabase[position].cardRarity)  // we save the dust value of each card into a list
                {
                    case "Legendary":
                        if (premium == "STANDARD")
                            DustPerCard.Add(400);
                        else
                            DustPerCard.Add(1600);
                        break;
                    case "Epic":
                        if (premium == "STANDARD")
                            DustPerCard.Add(100);
                        else
                            DustPerCard.Add(400);
                        break;
                    case "Rare":
                        if (premium == "STANDARD")
                            DustPerCard.Add(20);
                        else
                            DustPerCard.Add(100);
                        break;
                    case "Common":
                        if (premium == "STANDARD")
                            DustPerCard.Add(5);
                        else
                            DustPerCard.Add(50);
                        break;
                }
            }
        }

        private void cardCounter()
        {
            foreach (Card card in CardsFromPacks)
            {
                switch (card.cardRarity)
                {
                    case "Legendary":
                        if (card.cardPremium == "STANDARD")
                            noSLegendaries += card.cardCount;
                        else
                            noGLegendaries += card.cardCount;

                        if (card.cardCount > 1)
                            noELegendaries += card.cardCount - 1;
                        break;

                    case "Epic":
                        if (card.cardPremium == "STANDARD")
                            noSEpics += card.cardCount;
                        else
                            noGEpics += card.cardCount;

                        if (card.cardCount > 2)
                            noEEpics += card.cardCount - 2;
                        break;

                    case "Rare":
                        if (card.cardPremium == "STANDARD")
                            noSRares += card.cardCount;
                        else
                            noGRares += card.cardCount;

                        if (card.cardCount > 2)
                            noERares += card.cardCount - 2;
                        break;

                    case "Common":
                        if (card.cardPremium == "STANDARD")
                            noSCommons += card.cardCount;
                        else
                            noGCommons += card.cardCount;

                        if (card.cardCount > 2)
                            noECommons += card.cardCount - 2;
                        break;
                }
            }
            noACommons = noSCommons + noGCommons;
            noARares = noSRares + noGRares;
            noAEpics = noSEpics + noGEpics;
            noALegendaries = noSLegendaries + noGLegendaries;

            noSTotal = noSCommons + noSRares + noSEpics + noSLegendaries;
            noGTotal = noGCommons + noGRares + noGEpics + noGLegendaries;
            noETotal = noECommons + noERares + noEEpics + noELegendaries;
            noATotal = noACommons + noARares + noAEpics + noALegendaries;
        }

        private void dustCounter()
        {
            highestDustPack = 0;
            totalDust = 0;
            no40DustPacks = 0;
            totalPacks = 0;
            int dustPerPack, bestPackPos = -1;

            for (int i = 0; i < DustPerCard.Count(); i += 5)  // we surf the dust list grouping dust values 5 each (5 cards in a pack)
            {
                if (i + 4 < DustPerCard.Count())
                {
                    dustPerPack = DustPerCard[i] + DustPerCard[i + 1] + DustPerCard[i + 2] + DustPerCard[i + 3] + DustPerCard[i + 4];

                    if (dustPerPack > highestDustPack)
                    {
                        highestDustPack = dustPerPack;
                        bestPackPos = i;
                    }

                    if (dustPerPack == 40)
                        no40DustPacks++;

                    totalDust += dustPerPack;
                }
                else
                    break;
            }

            totalPacks = CardsFromPacksInOrder.Count() / 5;

            if (totalDust == 0)
                avgDustPerPack = 0;
            else
                avgDustPerPack = (float)totalDust / (DustPerCard.Count() / 5);

            extraCardsDust = noECommons * 5 + noERares * 20 + noEEpics * 100 + noELegendaries * 400;

            if (bestPackPos != -1)
            {
                bestPack.Add(CardsFromPacksInOrder[bestPackPos]);
                bestPack.Add(CardsFromPacksInOrder[bestPackPos+1]);
                bestPack.Add(CardsFromPacksInOrder[bestPackPos+2]);
                bestPack.Add(CardsFromPacksInOrder[bestPackPos+3]);
                bestPack.Add(CardsFromPacksInOrder[bestPackPos+4]);
            }
        }

        public int getCardPosition(string cardName)
        {
            List<Card> List = new List<Card>();

            for (int i = 0; i < CardDatabase.Count(); i++)
                if (CardDatabase[i].cardName == cardName)
                    return i;

            return -1;
        }

        public int getCardPosition(string cardName, string premium)
        {
            for (int i = 0; i < CardsFromPacks.Count(); i++)
                if (CardsFromPacks[i].cardName == cardName && CardsFromPacks[i].cardPremium == premium)
                    return i;

            return -1;
        }

        private void ResetCards()
        {
            CardsFromPacks.Clear();
            CardsFromPacksInOrder.Clear();
            DustPerCard.Clear();
        }

        private void ResetStats()
        {
            noSCommons = 0;
            noSRares = 0;
            noSEpics = 0;
            noSLegendaries = 0;
            noSTotal = 0;
            noGCommons = 0;
            noGRares = 0;
            noGEpics = 0;
            noGLegendaries = 0;
            noGTotal = 0;
            noECommons = 0;
            noERares = 0;
            noEEpics = 0;
            noELegendaries = 0;
            noETotal = 0;
            noACommons = 0;
            noARares = 0;
            noAEpics = 0;
            noALegendaries = 0;
            noATotal = 0;
            totalDust = 0;
            highestDustPack = 0;
            no40DustPacks = 0;
            totalPacks = 0;
            extraCardsDust = 0;
            avgDustPerPack = 0;
            bestPack.Clear();
            classicPositions.Clear();
            gvgPositions.Clear();
            tgtPositions.Clear();
        }

        public void saveToFile(string fileName)
        {
            try
            {
                if (!Directory.Exists(@".\Saves"))
                    Directory.CreateDirectory(@".\Saves");

                StreamWriter file = new StreamWriter(@".\Saves\" + fileName + ".sav");

                foreach (Card card in CardsFromPacksInOrder)
                    file.WriteLine(card.cardName + "|" + card.cardPremium);

                file.Close();
            }
            catch (Exception e)
            {
                StreamWriter file = new StreamWriter(DateTime.Now.ToString("yyyy-dd-MM--HH-mm-ss") + "_Crash_Log.txt");
                file.Write(e.ToString());
                file.Close();
                MessageBox.Show("Unhandled exception found. A log file containing the details of the exception has been saved to parent directory. If possible, send the log to the owner of this product using one of the means displayed in 'About'. Sorry for any inconveniences caused by this error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void loadFromFile(List<string> files)
        {
            ResetCards();
            try
            {
                foreach (string file in files)
                {
                    using (FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string text = reader.ReadToEnd();
                            string[] lines = text.Split('\n');
                            foreach (string line in lines)
                            {
                                if (line != "")
                                {
                                    string[] lineContent = line.Split('|');
                                    if (lineContent.Count() == 2)
                                    {
                                        if (lineContent[1].Contains('\r'))
                                            lineContent[1] = lineContent[1].Remove(lineContent[1].Length - 1);
                                        addNewCard(lineContent[0], lineContent[1]);
                                    }
                                }
                            }
                            reader.Close();
                        }
                        stream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                StreamWriter file = new StreamWriter(DateTime.Now.ToString("yyyy-dd-MM--HH-mm-ss") + "_Crash_Log.txt");
                file.Write(ex.ToString());
                file.Close();
                MessageBox.Show("Unhandled exception found. A log file containing the details of the exception has been saved to parent directory. If possible, send the log to the owner of this product using one of the means displayed in 'About'. Sorry for any inconveniences caused by this error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int getTotalCount(List<Card> list)
        {
            int nr = 0;
            foreach (Card card in list)
                nr += card.cardCount;
            return nr;
        }

        private List<Card> getCardsByCRPE(string cardClass, string cardRarity, string cardPremium, bool Extra)
        {
            List<Card> cards = new List<Card>();

            foreach (Card card in CardsFromPacks)
                if (Extra)
                {
                    int normalAmmount;

                    if (cardRarity == "Legendary")
                        normalAmmount = 1;
                    else
                        normalAmmount = 2;

                    if (card.cardClass == cardClass && card.cardRarity == cardRarity && card.cardPremium == cardPremium && card.cardCount > normalAmmount)
                    {
                        Card tempCard = card.Clone();
                        tempCard.cardCount = tempCard.cardCount - normalAmmount;
                        cards.Add(tempCard);
                    }
                }
                else
                if (card.cardClass == cardClass && card.cardRarity == cardRarity && card.cardPremium == cardPremium)
                    cards.Add(card);

            return cards;
        }

        private void calculateIndividualPacksPosition()
        {
            for (int i = 0; i < CardsFromPacksInOrder.Count(); i += 5)
            {
                switch (CardsFromPacksInOrder[i].cardSet)
                {
                    case "Classic": classicPositions.Add(i); break;
                    case "GVG": gvgPositions.Add(i); break;
                    case "TGT": tgtPositions.Add(i); break;
                }
            }
        }

        public void statsToFile(string fileName)
        {
            string bodySeparator = "--------------------------------------------";
            string titleSeparator = "============================================";

            if (!Directory.Exists(@".\Statistics"))
                Directory.CreateDirectory(@".\Statistics");

            StreamWriter file = new StreamWriter(@".\Statistics\" + fileName+".txt");

            file.WriteLine("Stats created on: " + DateTime.Now.ToString() + "\r\n");
            file.WriteLine(titleSeparator + "\r\nOVERALL STATISTICS\r\n" + titleSeparator + "\r\n");
            file.WriteLine(bodySeparator + "\r\nTotal packs opened: " + totalPacks + "\r\n" + bodySeparator);
            file.WriteLine("\r\n" + bodySeparator + "\r\nAll cards:\r\n" + bodySeparator);
            file.WriteLine("Commons: " + noACommons + " [" + ((float)(noACommons * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Rares: " + noARares + " [" + ((float)(noARares * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Epics: " + noAEpics + " [" + ((float)(noAEpics * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Legendaries: " + noALegendaries + " [" + ((float)(noALegendaries * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Total: " + noATotal + "\r\n" + bodySeparator);
            file.WriteLine("\r\n" + bodySeparator + "\r\nStandard cards:\r\n" + bodySeparator);
            file.WriteLine("Commons: " + noSCommons + " [" + ((float)(noSCommons * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Rares: " + noSRares + " [" + ((float)(noSRares * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Epics: " + noSEpics + " [" + ((float)(noSEpics * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Legendaries: " + noSLegendaries + " [" + ((float)(noSLegendaries * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Total: " + noSTotal + " [" + ((float)(noSTotal * 100) / noATotal).ToString("0.00") + "%]\r\n" + bodySeparator);
            file.WriteLine("\r\n" + bodySeparator + "\r\nGolden cards:\r\n" + bodySeparator);
            file.WriteLine("Commons: " + noGCommons + " [" + ((float)(noGCommons * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Rares: " + noGRares + " [" + ((float)(noGRares * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Epics: " + noGEpics + " [" + ((float)(noGEpics * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Legendaries: " + noGLegendaries + " [" + ((float)(noGLegendaries * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Total: " + noGTotal + " [" + ((float)(noGTotal * 100) / noATotal).ToString("0.00") + "%]\r\n" + bodySeparator);
            file.WriteLine("\r\n" + bodySeparator + "\r\nExtra cards:\r\n" + bodySeparator);
            file.WriteLine("Commons: " + noECommons + " [" + ((float)(noECommons * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Rares: " + noERares + " [" + ((float)(noERares * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Epics: " + noEEpics + " [" + ((float)(noEEpics * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Legendaries: " + noELegendaries + " [" + ((float)(noELegendaries * 100) / noATotal).ToString("0.00") + "%]");
            file.WriteLine("Total: " + noETotal + " [" + ((float)(noETotal * 100) / noATotal).ToString("0.00") + "%]\r\n" + bodySeparator);
            file.WriteLine("\r\n" + bodySeparator + "\r\nDust:\r\n" + bodySeparator);
            file.WriteLine("Total dust worth: " + totalDust);
            file.WriteLine("Average dust per pack: " + avgDustPerPack.ToString("0.00"));
            file.WriteLine("Extra cards dust worth: " + extraCardsDust);
            file.WriteLine("Number of 40 dust packs: " + no40DustPacks);
            file.WriteLine("Highest dust value in a pack: " + highestDustPack + "\r\n" + bodySeparator);
            file.WriteLine("\r\n" + bodySeparator + "\r\nBest pack:\r\n" + bodySeparator);
            if (bestPack.Count() > 0)
            {
                file.WriteLine("Set: " + bestPack[0].cardSet);

                file.WriteLine("Cards:");
                file.WriteLine("-" + bestPack[0].cardName + " [" + bestPack[0].cardClass + ", " + bestPack[0].cardRarity + ", " + bestPack[0].cardPremium + "]\r\n" +
                    "-" + bestPack[1].cardName + " [" + bestPack[1].cardClass + ", " + bestPack[1].cardRarity + ", " + bestPack[1].cardPremium + "]\r\n" +
                    "-" + bestPack[2].cardName + " [" + bestPack[2].cardClass + ", " + bestPack[2].cardRarity + ", " + bestPack[2].cardPremium + "]\r\n" +
                    "-" + bestPack[3].cardName + " [" + bestPack[3].cardClass + ", " + bestPack[3].cardRarity + ", " + bestPack[3].cardPremium + "]\r\n" +
                    "-" + bestPack[4].cardName + " [" + bestPack[4].cardClass + ", " + bestPack[4].cardRarity + ", " + bestPack[4].cardPremium + "]\r\n" + bodySeparator);
            }
            else
                file.WriteLine("Set: -\r\nCards:\r\n-\r\n-\r\n-\r\n-\r\n-");

            List<string> cardClasses = new List<string>() { "Druid", "Hunter", "Mage", "Paladin", "Priest", "Rogue", "Shaman", "Warlock", "Warrior", "Neutral" };

            file.WriteLine("\r\n\r\n" + titleSeparator + "\r\nCLASS BREAKDOWN\r\n" + titleSeparator);

            foreach (string cardClass in cardClasses)
            {
                List<Card> SCommons = getCardsByCRPE(cardClass, "Common", "STANDARD", false);
                List<Card> SRares = getCardsByCRPE(cardClass, "Rare", "STANDARD", false);
                List<Card> SEpics = getCardsByCRPE(cardClass, "Epic", "STANDARD", false);
                List<Card> SLegendaries = getCardsByCRPE(cardClass, "Legendary", "STANDARD", false);
                List<Card> GCommons = getCardsByCRPE(cardClass, "Common", "GOLDEN", false);
                List<Card> GRares = getCardsByCRPE(cardClass, "Rare", "GOLDEN", false);
                List<Card> GEpics = getCardsByCRPE(cardClass, "Epic", "GOLDEN", false);
                List<Card> GLegendaries = getCardsByCRPE(cardClass, "Legendary", "GOLDEN", false);
                List<Card> ESCommons = getCardsByCRPE(cardClass, "Common", "STANDARD", true);
                List<Card> ESRares = getCardsByCRPE(cardClass, "Rare", "STANDARD", true);
                List<Card> ESEpics = getCardsByCRPE(cardClass, "Epic", "STANDARD", true);
                List<Card> ESLegendaries = getCardsByCRPE(cardClass, "Legendary", "STANDARD", true);
                List<Card> EGCommons = getCardsByCRPE(cardClass, "Common", "GOLDEN", true);
                List<Card> EGRares = getCardsByCRPE(cardClass, "Rare", "GOLDEN", true);
                List<Card> EGEpics = getCardsByCRPE(cardClass, "Epic", "GOLDEN", true);
                List<Card> EGLegendaries = getCardsByCRPE(cardClass, "Legendary", "GOLDEN", true);

                SCommons = SCommons.OrderBy(o => o.cardName).ToList();
                SRares = SRares.OrderBy(o => o.cardName).ToList();
                SEpics = SEpics.OrderBy(o => o.cardName).ToList();
                SLegendaries = SLegendaries.OrderBy(o => o.cardName).ToList();
                GCommons = GCommons.OrderBy(o => o.cardName).ToList();
                GRares = GRares.OrderBy(o => o.cardName).ToList();
                GEpics = GEpics.OrderBy(o => o.cardName).ToList();
                GLegendaries = GLegendaries.OrderBy(o => o.cardName).ToList();
                ESCommons = ESCommons.OrderBy(o => o.cardName).ToList();
                ESRares = ESRares.OrderBy(o => o.cardName).ToList();
                ESEpics = ESEpics.OrderBy(o => o.cardName).ToList();
                ESLegendaries = ESLegendaries.OrderBy(o => o.cardName).ToList();
                EGCommons = EGCommons.OrderBy(o => o.cardName).ToList();
                EGRares = EGRares.OrderBy(o => o.cardName).ToList();
                EGEpics = EGEpics.OrderBy(o => o.cardName).ToList();
                EGLegendaries = EGLegendaries.OrderBy(o => o.cardName).ToList();

                int SCommonsCount, GCommonsCount, SRaresCount, GRaresCount, SEpicsCount, GEpicsCount, SLegendariesCount, GLegendariesCount, ECommonsCount, ERaresCount, EEpicsCount, ELegendariesCount;

                SCommonsCount = getTotalCount(SCommons);
                GCommonsCount = getTotalCount(GCommons);
                ECommonsCount = getTotalCount(ESCommons.Concat(EGCommons).ToList());
                SRaresCount = getTotalCount(SRares);
                GRaresCount = getTotalCount(GRares);
                ERaresCount = getTotalCount(ESRares.Concat(EGRares).ToList());
                SEpicsCount = getTotalCount(SEpics);
                GEpicsCount = getTotalCount(GEpics);
                EEpicsCount = getTotalCount(ESEpics.Concat(EGEpics).ToList());
                SLegendariesCount = getTotalCount(SLegendaries);
                GLegendariesCount = getTotalCount(GLegendaries);
                ELegendariesCount = getTotalCount(ESLegendaries.Concat(EGLegendaries).ToList());

                int total = SCommonsCount + GCommonsCount + SRaresCount + GRaresCount + SEpicsCount + GEpicsCount + SLegendariesCount + GLegendariesCount;

                file.WriteLine("\r\n" + bodySeparator + "\r\n" + cardClass + ": " + total + " [" + ((float)(total * 100) / noATotal).ToString("0.00") + "%]\r\n" + bodySeparator);
                file.WriteLine("Commons: " + (SCommonsCount + GCommonsCount) + " [" + ((float)((SCommonsCount + GCommonsCount) * 100) / noATotal).ToString("0.00") + "%] < STANDARD: " +
                    SCommonsCount + " [" + ((float)(SCommonsCount * 100) / noATotal).ToString("0.00") + "%], GOLDEN: " + GCommonsCount + " [" + ((float)(GCommonsCount * 100) / noATotal).ToString("0.00") + "%], EXTRA CARDS: " +
                    ECommonsCount + " [" + ((float)(ECommonsCount * 100) / noATotal).ToString("0.00") + "%] >\r\n\r\nStandard:");

                foreach (Card card in SCommons)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + "] x" + card.cardCount);

                file.WriteLine("\r\nGolden: ");

                foreach (Card card in GCommons)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + "] x" + card.cardCount);

                file.WriteLine("\r\nExtra cards: ");

                foreach (Card card in ESCommons)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + ", " + card.cardPremium + "] x" + card.cardCount);
                foreach (Card card in EGCommons)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + ", " + card.cardPremium + "] x" + card.cardCount);

                file.WriteLine(bodySeparator + "\r\nRares: " + (SRaresCount + GRaresCount) + " [" + ((float)((SRaresCount + GRaresCount) * 100) / noATotal).ToString("0.00") + "%] < STANDARD: " +
                       SRaresCount + " [" + ((float)(SRaresCount * 100) / noATotal).ToString("0.00") + "%], GOLDEN: " + GRaresCount + " [" + ((float)(GRaresCount * 100) / noATotal).ToString("0.00") + "%], EXTRA CARDS: " +
                    ERaresCount + " [" + ((float)(ERaresCount * 100) / noATotal).ToString("0.00") + "%] >\r\n\r\nStandard:");

                foreach (Card card in SRares)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + "] x" + card.cardCount);

                file.WriteLine("\r\nGolden: ");

                foreach (Card card in GRares)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + "] x" + card.cardCount);

                file.WriteLine("\r\nExtra cards: ");

                foreach (Card card in ESRares)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + ", " + card.cardPremium + "] x" + card.cardCount);
                foreach (Card card in EGRares)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + ", " + card.cardPremium + "] x" + card.cardCount);

                file.WriteLine(bodySeparator + "\r\nEpics: " + (SEpicsCount + GEpicsCount) + " [" + ((float)((SEpicsCount + GEpicsCount) * 100) / noATotal).ToString("0.00") + "%] < STANDARD: " +
                 SEpicsCount + " [" + ((float)(SEpicsCount * 100) / noATotal).ToString("0.00") + "%], GOLDEN: " + GEpicsCount + " [" + ((float)(GEpicsCount * 100) / noATotal).ToString("0.00") + "%], EXTRA CARDS: " +
              EEpicsCount + " [" + ((float)(EEpicsCount * 100) / noATotal).ToString("0.00") + "%] >\r\n\r\nStandard:");

                foreach (Card card in SEpics)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + "] x" + card.cardCount);

                file.WriteLine("\r\nGolden: ");

                foreach (Card card in GEpics)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + "] x" + card.cardCount);

                file.WriteLine("\r\nExtra cards: ");

                foreach (Card card in ESEpics)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + ", " + card.cardPremium + "] x" + card.cardCount);
                foreach (Card card in EGEpics)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + ", " + card.cardPremium + "] x" + card.cardCount);

                file.WriteLine(bodySeparator + "\r\nLegendaries: " + (SLegendariesCount + GLegendariesCount) + " [" + ((float)((SLegendariesCount + GLegendariesCount) * 100) / noATotal).ToString("0.00") + "%] < STANDARD: " +
                 SLegendariesCount + " [" + ((float)(SLegendariesCount * 100) / noATotal).ToString("0.00") + "%], GOLDEN: " + GLegendariesCount + " [" + ((float)(GLegendariesCount * 100) / noATotal).ToString("0.00") + "%], EXTRA CARDS: " +
              ELegendariesCount + " [" + ((float)(ELegendariesCount * 100) / noATotal).ToString("0.00") + "%] >\r\n\r\nStandard:");

                foreach (Card card in SLegendaries)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + "] x" + card.cardCount);

                file.WriteLine("\r\nGolden: ");

                foreach (Card card in GLegendaries)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + "] x" + card.cardCount);

                file.WriteLine("\r\nExtra cards: ");

                foreach (Card card in ESLegendaries)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + ", " + card.cardPremium + "] x" + card.cardCount);
                foreach (Card card in EGLegendaries)
                    file.WriteLine("-" + card.cardName + " [" + card.cardSet + ", " + card.cardPremium + "] x" + card.cardCount);
                file.WriteLine(bodySeparator);
            }


            file.Close();
        }
    }
}
