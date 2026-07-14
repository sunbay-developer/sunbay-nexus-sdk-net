#!/usr/bin/env python3
import argparse
import getpass
import re
import subprocess
import sys
from pathlib import Path


def run_command(command: list[str], cwd: Path) -> None:
    print(f"\n>>> {' '.join(command)}")
    subprocess.run(command, cwd=cwd, check=True)


def extract_xml_tag(content: str, tag: str) -> str | None:
    match = re.search(rf"<{tag}>\s*([^<]+?)\s*</{tag}>", content)
    return match.group(1).strip() if match else None


def main() -> int:
    parser = argparse.ArgumentParser(
        description="One-click pack and publish Sunbay.Nexus.Sdk to NuGet."
    )
    parser.add_argument(
        "--project",
        default="src/Sunbay.Nexus.Sdk/Sunbay.Nexus.Sdk.csproj",
        help="Relative path to the .csproj file.",
    )
    parser.add_argument(
        "--output",
        default="nupkgs",
        help="Output directory for packed .nupkg files.",
    )
    parser.add_argument(
        "--source",
        default="https://api.nuget.org/v3/index.json",
        help="NuGet push source URL.",
    )
    parser.add_argument(
        "--skip-duplicate",
        action="store_true",
        default=True,
        help="Skip push if the package version already exists (default: enabled).",
    )
    parser.add_argument(
        "--no-skip-duplicate",
        dest="skip_duplicate",
        action="store_false",
        help="Disable --skip-duplicate when pushing package.",
    )
    args = parser.parse_args()

    repo_root = Path(__file__).resolve().parent
    project_path = (repo_root / args.project).resolve()
    output_dir = (repo_root / args.output).resolve()

    if not project_path.exists():
        print(f"Error: project file not found: {project_path}", file=sys.stderr)
        return 1

    project_content = project_path.read_text(encoding="utf-8")
    package_id = extract_xml_tag(project_content, "PackageId")
    version = extract_xml_tag(project_content, "Version")

    if not package_id:
        print("Error: <PackageId> not found in project file.", file=sys.stderr)
        return 1

    if not version:
        print("Error: <Version> not found in project file.", file=sys.stderr)
        return 1

    print(f"Package: {package_id}")
    print(f"Version: {version}")
    print(f"Project: {project_path}")

    api_key = getpass.getpass("NuGet API Key (input hidden): ").strip()
    if not api_key:
        print("Error: API Key is required.", file=sys.stderr)
        return 1

    output_dir.mkdir(parents=True, exist_ok=True)

    try:
        run_command(
            [
                "dotnet",
                "pack",
                str(project_path),
                "-c",
                "Release",
                "-o",
                str(output_dir),
                "--nologo",
                "--verbosity",
                "minimal",
            ],
            cwd=repo_root,
        )

        nupkg_path = output_dir / f"{package_id}.{version}.nupkg"
        if not nupkg_path.exists():
            print(f"Error: package not found after pack: {nupkg_path}", file=sys.stderr)
            return 1

        push_command = [
            "dotnet",
            "nuget",
            "push",
            str(nupkg_path),
            "--api-key",
            api_key,
            "--source",
            args.source,
        ]
        if args.skip_duplicate:
            push_command.append("--skip-duplicate")

        run_command(push_command, cwd=repo_root)
        print(f"\nDone: published {package_id} {version}")
        return 0
    except subprocess.CalledProcessError as error:
        print(f"\nPublish failed with exit code {error.returncode}.", file=sys.stderr)
        return error.returncode


if __name__ == "__main__":
    raise SystemExit(main())
