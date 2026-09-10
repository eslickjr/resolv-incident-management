import { useEffect, useState, useImperativeHandle, forwardRef } from 'react';
import { useMsal } from '@azure/msal-react';
import { IncidentNote } from '../interfaces/Types';
import { getNotes } from '../api/incidents';
import '../styles/Notes.css';

interface NotesProps {
    incidentId?: number;
    isReadOnly?: boolean;
    noteText?: string;
    onNoteChange?: (val: string) => void;
}

export interface NotesRef {
    refreshNotes: () => void;
}

const Notes = forwardRef<NotesRef, NotesProps>(({ incidentId, isReadOnly = false, noteText = '', onNoteChange }, ref) => {
    const { instance } = useMsal();
    const [notes, setNotes] = useState<IncidentNote[]>([]);
    const [loading, setLoading] = useState<boolean>(true);

    const fetchNotes = async () => {
        if (!incidentId) { setLoading(false); return; }
        setLoading(true);
        try {
            const data = await getNotes(instance, incidentId);
            setNotes(data);
        } catch (err) {
            console.error('Failed to load notes:', err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        console.log('Notes useEffect fired, incidentId:', incidentId);
        fetchNotes();
    }, [incidentId]);

    // Expose refreshNotes to parent via ref
    useImperativeHandle(ref, () => ({
        refreshNotes: fetchNotes
    }));

    const formatDate = (isoString: string): string => {
        // Ensure UTC is indicated so browser converts to local time correctly
        const utcString = isoString.endsWith('Z') ? isoString : isoString + 'Z';
        const date = new Date(utcString);
        return date.toLocaleDateString('en-US', {
            month: '2-digit',
            day: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    if (!incidentId) return null;

    return (
        <div id="notesContainer">
            <div id="notesHeader">
                <span id="notesTitle">Notes</span>
            </div>
            {!isReadOnly && (
                <div id="noteInputContainer">
                    <textarea
                        id="noteInput"
                        value={noteText}
                        onChange={e => onNoteChange?.(e.target.value)}
                        placeholder="Add a note..."
                        rows={3}
                    />
                </div>
            )}
            <div id="notesList">
                {loading ? (
                    <p className="notesEmpty">Loading...</p>
                ) : notes.length === 0 ? (
                    <p className="notesEmpty">No notes yet.</p>
                ) : (
                    notes.map(note => (
                        <div key={note.noteId} className="noteItem">
                            <div className="noteMeta">
                                <span className="noteUsername">{note.username}</span>
                                <span className="noteDate">{formatDate(note.createdAt)}</span>
                            </div>
                            <p className="noteText">{note.note}</p>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
});

Notes.displayName = 'Notes';
export default Notes;