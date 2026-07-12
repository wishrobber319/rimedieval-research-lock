# Rimedieval - Enforced Research Lock

Rimedieval only hides disallowed (too-advanced) research projects from the vanilla research tab - it never actually makes them unstartable, so their CanStartNow stays true. Mods like Research Whatever pick any project whose CanStartNow is true, so they happily research the hidden ones, bypassing Rimedieval's restriction.

This patch makes Rimedieval-disallowed projects fail CanStartNow, so Research Whatever (and any other "research anything" path, e.g. research-tree queueing) respects the lock. It reuses Rimedieval's own allowed-projects rule, so it honors Rimedieval's tech-level setting and its Odyssey exception. Changing that Rimedieval setting takes effect on the next restart.

---

A RimWorld 1.6 mod by wishRobber.
