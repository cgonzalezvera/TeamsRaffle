# Setup and Config

The project targets .NET Framework 4.5 and requires Visual Studio 2013+ or MSBuild to build.

## Running the Application

- The executable expects a `config.txt` file in the current directory with format: `players=c:\path\to\players.txt,results=c:\path\to\output.txt`
- Paths must be absolute; relative paths may not resolve correctly.

## File Formats

### Players Input File
- Single text file containing player entries separated by commas.
- Each entry: `name[=skill]`
- Skill is optional integer (defaults to 1).
- Example: `alice[=5],bob,charlie[=3]`
- Must have even number of players.

### Output
- Text file with comma-separated teams, one per line.
- Format: `Equipo1:[alice, bob], Equipo2:[charlie, dave]`

## Algorithm Notes
- Assigns random weights based on player skill and current time.
- Teams formed by weight partition, then balanced by length.
- Results are non-deterministic; re-running may yield different teams.