# BACKUP & RESTORE

These commands allow to (auto)backup tags of all tracks in your library and restore them. First, create full tag backup manually. You can do manual or auto incremental backups after this.

*Note:*

* "Backup \& Restore" relies on MusicBee internal track IDs specific to the library. Because of this, you can't restore tags to a library other than the library from which the tags have been backed up. But you have an option for the plugin to save a copy of the track IDs to a custom tag of your choice, and to restore backed-up tags using this custom tag to identify tracks instead of actual track IDs. This will let you restore the tags to the new library containing the same tracks if you have recreated the library from scratch, e.g., on another computer. Make sure that this custom tag is stored in the file tag, not in the MusicBee database, and use plugin menu item "Create Empty Baseline and Restore Tags from Another Library".

***

Copyright © boroda 2012-2024. Help version 9.2.240921