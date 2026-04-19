# PackagingTenderTool

## Formål
PackagingTenderTool er et prototypeprojekt til evaluering af leverandørtilbud inden for packaging tender-materiale.

Projektet er udviklet for at gøre tender-vurdering mere struktureret og mere genbrugelig end en ren Excel-baseret proces. 
Fokus er på import, datavalidering, datarensning, analytics og et markant tydeligere beslutningsgrundlag.

## Hvad løsningen kan nu
Løsningen understøtter i dag:

- import af tender-data fra Excel
- validering af input og importrapportering
- datarensning og normalisering
- analyse af tender-data og spend
- frontend-ready dashboard/view-model kontrakter
- eksportretning via CSV for udvalgte outputs
- automatiske tests af kernefunktionalitet

## Arkitekturretning
Projektet er flyttet væk fra tung WinForms-finpolering.

Den nuværende WinForms-applikation fungerer som en midlertidig prototype og demonstrationsflade. Den egentlige værdi flyttes nu over i en mere genbrugelig arkitektur med fokus på:

- **Core**
- **Import**
- **Analytics**
- **frontend-ready view models**

Retningen fremad er en fremtidig **Blazor-frontend med Radzen**, så brugerfladen kan bygges på en mere velegnet platform til data- og dashboardvisning.

## Projektstruktur
### `src/PackagingTenderTool.Core`
Kerneprojektet indeholder:
- domænemodeller
- importlogik
- validering
- datarensning
- analytics
- dashboard/view-model kontrakter

### `src/PackagingTenderTool.App`
WinForms-applikationen bruges som midlertidig prototype til at demonstrere, at motoren virker.

### `tests/PackagingTenderTool.Core.Tests`
Automatiske tests for kernefunktionalitet, import, rensning, analytics og frontend-ready kontrakter.

## Hvorfor retningen blev ændret
WinForms var nyttig til hurtigt at få en prototype op at køre, men yderligere GUI-finjustering gav faldende værdi.

Projektet er derfor bevidst drejet mod:
- tydeligere separation of concerns
- mere genbrugelig kode
- stærkere datalag og analytics
- forberedelse til Blazor + Radzen

Det gør løsningen mere velegnet til videreudvikling og bedre som programmeringsfagligt projekt.

## Faglig relevans
Projektet er relevant i programmeringsfaget, fordi det arbejder med:

- decomposition af et komplekst problem
- separation of concerns
- import og validering
- datarensning og normalisering
- analytics og beregningslogik
- testbarhed
- genbrugelige services og modeller
- forberedelse til brug af eksterne frontend-biblioteker

## Nuværende status
Den aktuelle version i repository er den gældende version.

Status lige nu:
- `main` er synkroniseret
- build er grøn
- tests er grønne
- der er **56 automatiske tests**
- næste dokumentationsskridt er opdatering af `plan.md` og `spec.md`

## Mulige videreudviklinger
- Blazor-frontend med Radzen
- mere avanceret filtrering og drill-down
- stærkere eksportfunktioner
- tydeligere breakdowns pr. land, site, materiale og størrelse
- videreudvikling af tender-logik og leverandørsammenligning