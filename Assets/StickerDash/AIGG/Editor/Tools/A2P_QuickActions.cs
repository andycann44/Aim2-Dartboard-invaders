#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;

namespace Aim2Pro.Tools {
  public static class A2P_QuickActions {
    static string ProjectRoot => Directory.GetCurrentDirectory();

    static int RunShell(string cmd) {
      var p = new Process();
      p.StartInfo.FileName = "/bin/bash";
      p.StartInfo.Arguments = "-lc \"" + cmd.Replace("\"","\\\"") + "\"";
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.RedirectStandardError = true;
      p.StartInfo.CreateNoWindow = true;
      p.Start();
      string out1 = p.StandardOutput.ReadToEnd();
      string err1 = p.StandardError.ReadToEnd();
      p.WaitForExit();
      UnityEngine.Debug.Log($"$ {cmd}\n{out1}{err1}");
      return p.ExitCode;
    }

    // --- Terminal at project root (⌘T inside Unity) ---
    [MenuItem("Window/Aim2Pro/Tools/Terminal/Open at Project Root %t", false, 10)]
    public static void OpenTerminalAtProject() {
      // Prefer iTerm if installed; otherwise Terminal
      if (Directory.Exists("/Applications/iTerm.app"))
        RunShell($"open -a iTerm \"{ProjectRoot}\"");
      else
        RunShell($"open -a Terminal \"{ProjectRoot}\"");
    }

    // --- GitHub: Save -> Commit -> Push (uses a2p_save.sh if present) ---
    [MenuItem("Window/Aim2Pro/Tools/GitHub/Save: Snapshot → Commit → Push", false, 20)]
    public static void SaveCommitPush() {
      string msg = $"quick-save {System.DateTime.Now:yyyy-MM-dd_HH-mm}";
      var helper = Path.Combine(ProjectRoot, "a2p_save.sh");
      if (File.Exists(helper)) RunShell($"\"{helper}\" \"{msg}\"");
      else {
        RunShell("[ -f ./a2p_snap.sh ] && ./a2p_snap.sh || true");
        RunShell("git add -A");
        RunShell($"git commit -m \"{msg}\" || true");
        RunShell("git push");
      }
    }

    [MenuItem("Window/Aim2Pro/Tools/GitHub/Open Repo (web)", false, 21)]
    public static void OpenRepoWeb() {
      // Try gh; fallback to origin URL
      var code = RunShell("command -v gh >/dev/null && gh repo view --web || true");
      if (code != 0) {
        RunShell("open $(git config --get remote.origin.url | sed 's/.git$//' | sed 's#git@#https://#; s#:/#/#')");
      }
    }

    [MenuItem("Window/Aim2Pro/Tools/GitHub/Open PRs (web)", false, 22)]
    public static void OpenPRsWeb() {
      var code = RunShell("command -v gh >/dev/null && gh pr list --web || true");
      if (code != 0) {
        RunShell("open $(git config --get remote.origin.url | sed 's/.git$//' | sed 's#git@#https://#; s#:/#/#')/pulls");
      }
    }

    [MenuItem("Window/Aim2Pro/Tools/GitHub/Create PR (current branch)", false, 23)]
    public static void CreatePR() {
      RunShell("command -v gh >/dev/null && gh pr create -f || open $(git config --get remote.origin.url | sed 's/.git$//' | sed 's#git@#https://#; s#:/#/#')/compare");
    }

    [MenuItem("Window/Aim2Pro/Tools/GitHub/Tag & Push v<bundleVersion>", false, 24)]
    public static void TagAndPushFromPlayerSettings() {
      string ver = PlayerSettings.bundleVersion;
      if (string.IsNullOrEmpty(ver)) {
        EditorUtility.DisplayDialog("Aim2 — Tag & Push", "PlayerSettings.bundleVersion is empty. Set it (e.g., 0.1.0) in Project Settings → Player.", "OK");
        return;
      }
      RunShell($"git tag -a v{ver} -m \"Release v{ver}\" || true");
      RunShell($"git push origin v{ver} || true");
      // Optional: create a release (no assets attached)
      RunShell($"command -v gh >/dev/null && gh release create v{ver} -n \"Auto release v{ver}\" || true");
      EditorUtility.DisplayDialog("Aim2 — Tag & Push", $"Tagged and pushed v{ver}.", "OK");
    }
  }
}
#endif
