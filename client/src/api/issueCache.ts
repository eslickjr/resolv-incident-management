import { IPublicClientApplication } from '@azure/msal-browser';
import { getAccessToken } from '../auth/getAccessToken';

export interface IssueOption {
    issueId: number;
    name: string;
}

export interface SolutionOption {
    solutionId: number;
    issueId: number;
    name: string;
}

export interface CallTipOption {
    tipId: number;
    issueId: number;
    tip: string;
    sortOrder: number;
}

let issues: IssueOption[] = [];
let solutions: SolutionOption[] = [];
let callTips: CallTipOption[] = [];
let loaded = false;

export const loadIssuesAndSolutions = async (instance: IPublicClientApplication) => {
    if (loaded) return;
    const token = await getAccessToken(instance);
    const headers = { 'Authorization': `Bearer ${token}` };

    const [issueRes, solRes, tipsRes] = await Promise.all([
        fetch('/api/issues', { headers }),
        fetch('/api/issues/solutions', { headers }),
        fetch('/api/issues/tips', { headers })
    ]);

    issues = await issueRes.json();
    solutions = await solRes.json();
    callTips = await tipsRes.json();
    loaded = true;
};

export const getIssues = (): IssueOption[] => issues;

export const getSolutionsForIssue = (issueId: number): SolutionOption[] =>
    solutions.filter(s => s.issueId === issueId);

export const getCallTipsForIssue = (issueId: number): CallTipOption[] =>
    callTips.filter(t => t.issueId === issueId).sort((a, b) => a.sortOrder - b.sortOrder);

