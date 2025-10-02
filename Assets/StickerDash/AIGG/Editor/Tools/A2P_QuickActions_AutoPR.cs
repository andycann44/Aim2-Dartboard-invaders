#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;

namespace Aim2Pro.Tools {
  public static class A2P_QuickActions_AutoPR {
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

    // Cmd+Shift+P inside Unity
    [MenuItem("Window/Aim2Pro/Tools/GitHub/Save → Commit → Push → PR (Auto) %#p", false, 25)]
    public static void AutoPR() {
      var helper = Path.Combine(ProjectRoot, "a2p_auto_pr.sh");
      string msg = $"auto-save {System.DateTime.Now:yyyy-MM-dd_HH-mm}";
      if (File.Exists(helper)) {
        RunShell($"\"{helper}\" \"{msg}\"");
      } else {
        // Fallback inline (no helper found)
        RunShell("[ -f ./a2p_snap.sh ] && ./a2p_snap.sh || true");
        RunShell("git add -A");
        RunShell($"git commit -m \"{msg}\" || true");
        RunShell("BR=$(git symbolic-ref --short HEAD 2>/dev/null || echo a2p/$(date +%Y-%m-%d)-auto); git checkout -b \"$BR\" || git checkout \"$BR\"; git push -u origin \"$BR\"");
        RunShell("BASE=$(git symbolic-ref refs/remotes/origin/HEAD 2>/dev/null | sed 's@^refs/remotes/origin/@@' || echo main); command -v gh >/dev/null && (gh pr view >/dev/null 2>&1 || gh pr create -t \"$(git log -1 --pretty=%s)\" -b \"Auto PR\" -B \"$BASE\") || true");
      }
      EditorUtility.DisplayDialog("Aim2 — Auto PR", "Saved, pushed, and PR handled (created or opened).", "OK");
    }
  }
}
#endif
