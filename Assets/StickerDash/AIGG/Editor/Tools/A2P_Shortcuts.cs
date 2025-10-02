#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using System.IO;

namespace Aim2Pro.Tools {
  public static class A2P_Shortcuts {
    // Primary: Cmd (Action) + T
    [Shortcut("Aim2Pro/Open Terminal at Project Root", KeyCode.T, ShortcutModifiers.Action)]
    static void OpenTerminalShortcut() {
      A2P_QuickActions.OpenTerminalAtProject();
    }

    // Backup: Cmd+Shift+T (use if Cmd+T is conflicting)
    [Shortcut("Aim2Pro/Open Terminal at Project Root (Backup)", KeyCode.T, ShortcutModifiers.Action | ShortcutModifiers.Shift)]
    static void OpenTerminalShortcutBackup() {
      // Only fire if primary is not bound / disabled
      // Safe to call same action; Shortcut Manager resolves which binding is active.
    }

    // Optional: Auto PR via Cmd+Shift+P (matches your menu)
    [Shortcut("Aim2Pro/GitHub Auto PR", KeyCode.P, ShortcutModifiers.Action | ShortcutModifiers.Shift)]
    static void AutoPRShortcut() {
      A2P_QuickActions_AutoPR.AutoPR();
    }
  }
}
#endif
