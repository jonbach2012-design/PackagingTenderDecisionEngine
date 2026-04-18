PackagingTenderTool
Formål

Dette projekt er udviklet som en prototype til evaluering af leverandørtilbud (tenders) inden for emballage – med fokus på labels (Labels v1).
Formålet er at kunne importere data fra Excel, evaluere leverandører på flere parametre og præsentere et samlet beslutningsgrundlag.

Funktionalitet

Applikationen kan:

importere tender-data fra Excel
håndtere forskellige talformater (fx 1.250,50 og 1,250.50)
evaluere leverandører på:
Commercial
Technical
Regulatory (PPWR/EPR inspireret)
beregne samlet score (vægtet)
klassificere leverandører:
Recommended
Conditional
ManualReview
håndtere manglende eller ugyldige data via Manual Review flags
vise resultater via GUI (WinForms)
Arkitektur (overblik)

Projektet er opdelt i flere lag for at sikre struktur og genbrug:

Core (forretningslogik)

Indeholder:

domænemodeller (fx Tender, Supplier, LabelLineItem)
services til:
evaluering (LineEvaluationService)
aggregering (SupplierAggregationService)
klassifikation (SupplierClassificationService)
scoreberegning
App (UI + entry point)
WinForms GUI (MainForm)
filvalg + evaluering
visning af resultater pr. leverandør
Import
Excel-import via ClosedXML
mapping fra Excel → domænemodeller
håndtering af datakvalitet og regionale formater
Tests
unit tests for:
scoring
import
evaluering
sikrer at ændringer ikke ødelægger eksisterende funktionalitet
Programmeringsmæssig kompleksitet

Projektet demonstrerer brug af flere centrale programmeringsstrukturer:

objektorienteret design (flere klasser med ansvar)
separation of concerns (models, services, UI)
collections (lister af linjer, leverandører, flags)
betinget logik (if/else i scoring og klassifikation)
beregninger og vægtet aggregering
datavalidering og fejlhåndtering
filimport og parsing
GUI-interaktion
unit testing

Det gør projektet væsentligt mere komplekst end en simpel konsolapplikation.

Status

Projektet er en første fungerende prototype (v1):

✔ Import virker
✔ Scoring virker (Commercial / Technical / Regulatory)
✔ Klassifikation virker
✔ GUI findes
✔ Unit tests dækker kernefunktionalitet

Næste skridt:

forbedre GUI
teste med rigtige data
finpudse regler og validering

Kør projektet
dotnet build PackagingTenderTool.sln
dotnet run --project src/PackagingTenderTool.App/PackagingTenderTool.App.csproj



Kort sagt

Projektet viser, hvordan man kan kombinere:

data → logik → evaluering → beslutningsgrundlag → visning

… i en struktureret og testbar løsning.