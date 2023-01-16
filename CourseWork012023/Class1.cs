using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CourseWork012023
{
    public struct edgeinfo
    {
        public int weight;
        public string endname; //id of node on the end

        public edgeinfo (string endname, int weight)
        {
            this.weight = weight;
            this.endname = endname;
        }
    }

    public struct nodeinfo
    {
        public string name;
        public List<edgeinfo> neighbors; //Node's adjacency list

        public nodeinfo(int ID)
        {
            this.name = ID.ToString();
            this.neighbors = new List<edgeinfo>();
        }
        public nodeinfo(string Name)
        {
            this.name = Name;
            this.neighbors = new List<edgeinfo>();
        }

    }

    //Multigraph data: adjacency list
    public class GraphData: Microsoft.Msagl.Drawing.Graph
    {
        protected List<nodeinfo> nodes; // List of nodes
        public int nodecounts
        {
            get { return nodes.Count; }
        }
        
        public GraphData()
        {
            nodes = new List<nodeinfo>();
        }

        public void NewNode()
        {
            nodes.Add(new nodeinfo(nodes.Count));
            base.AddNode((nodes.Count - 1).ToString());
        }

        //Converting adjacency list to a string for form2
        public string GetData()
        {
            StringBuilder sb = new StringBuilder("");

            foreach(var node in nodes)
            {
                sb.Append(node.name + ": "); // node's name
                foreach(var edge in node.neighbors)
                {
                    sb.Append(edge.endname); // edge end name
                    if(edge.weight > 1) 
                        sb.Append(" (" + edge.weight + ")"); //weight
                    sb.Append(", ");
                }
                sb.Append("\n");

            }


            return sb.ToString();
        }

        public void Intersection(string newdata)
        {
            //Splitting by Adjacency List rows ('nodeinfo')
            string[] adjRows = newdata.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            DeleteNotCommon(adjRows);
            foreach ( var i in adjRows)
            { 
                //Division by Name and Neighbors
                string s = RemoveWhitespace(i); // delete all spaces
                string[] strNodeInfo = s.Split(':');// NAME: OTHER, DATA (EXAMPLE)
                //too much ':'
                if(strNodeInfo.Length > 2)
                    throw new Exception("В строке списка должна быть ровно одна вершина");
                //empty string
                if(strNodeInfo.Length < 2)
                    continue;

                //Finding node in 'nodes'
                int index = FindNodeWithName(strNodeInfo[0]);
                // create node if doesn't exist
                if (index == -1)
                    NewNode(strNodeInfo[0]);
                index = FindNodeWithName(strNodeInfo[0]);
                //base.Attr

                //Splitting of neighbors
                string[] strneighbors = strNodeInfo[1].Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach (var j in strneighbors)
                {
                    int weight = 1; // default value
                    string edgeEndName = j; //adjacency node's name
                    if (j.Contains('(') && j.Contains(')'))
                    {
                        //Splitting = {'name_of_edge_end', '(', 'weight_of_edge', ')'}
                        char[] sep = { '(', ')' };
                        string[] edge = j.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        if (edge.Length > 2 && edge.Length == 1)
                            throw new Exception("Неверно введён формат");
                        System.Int32.TryParse(edge[1], out weight);
                        weight = weight == 0 ? 1 : weight;
                        edgeEndName = edge[0];
                    }
                    if (FindNodeWithName(edgeEndName)== -1)
                        NewNode(edgeEndName);

                    NewEdge(index, edgeEndName, weight);
                }
            }
            nodes.Sort((x,y) => x.name.CompareTo(y.name));
            return;
        }
        

        public void NewNode(string Name)
        {
            Console.WriteLine("New node", Name);
            //name uniqueness check
            int delpos = FindNodeWithName(Name);
            if (delpos < 0)
            {
                nodes.Add(new nodeinfo(Name));
            }
            else
                nodes.Add(new nodeinfo(nodes.Count));
        }
        public void DeleteNode(string Name)
        {

        }
        
        public void PopNode()
        {
            if (nodes.Count == 0)
                return;
            nodes = DeleteRemains( new List<string>() {nodes[nodes.Count -1].name}, nodes);
            nodes.RemoveAt(nodes.Count - 1);

            Console.WriteLine("Pop node");

        }
        public void DeleteEdge(string A, string B, int weight = 1)
        {
            int posA = FindNodeWithName(A);
            if (posA == -1)
                return;
            int posB = FindNodeWithName(B);
            if (posB == -1)
                return;

            if (nodes[posA].neighbors.Count == 0)
                return;

            int edgeNo = FindEdgeWhithWeight(nodes[posA], weight); // Is there and where 
            if (edgeNo != -1)
                nodes[posA].neighbors.RemoveAt(edgeNo);

            Console.WriteLine("Delete edge ", A, " ", B, " ", weight.ToString());

        }

        /** PROTECTED MEMBERS*/
        // Finding the position of node whith such name
        //  return: (-1) in case doesn't found; (-i) in case repeating of values (for uniqueness)
        protected int FindNodeWithName(string name)
        {
            int foundposition = -1; // in case there is no such node
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].name == name)
                    if (foundposition == -1) // if it's first time
                        foundposition = i;
                    else // there are repeats
                    {
                        foundposition = -i; //in case there is more than 1
                        break;
                    }
            }
            return foundposition;
        }
        protected void NewEdge(int posA, string B, int weight = 1)
        {
            //Node existing check
            if (posA > nodes.Count || posA < 0)
                return;



            // multigraph adding
            nodes[posA].neighbors.Add(new edgeinfo(B, weight));
            Console.WriteLine("New edge ", posA, " ", B, " ", weight.ToString());

        }
        //finding position of edge in node's adjacency list (a.k.a. 'neighbors')
        //return: position; (-1) if there isn't such edge
        protected int FindEdgeWhithWeight(nodeinfo Ainf, int weight)
        {
            for(int i = 0; i< Ainf.neighbors.Count; i++)
            {
                if (Ainf.neighbors[i].weight == weight)
                    return i;
            }
            return -1;
        }
        protected string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }
        //Deleting all of old graph nodes
        //Compares new adjacency list with old and delete not matches
        // return List
        protected List<nodeinfo> DeleteNotCommon(string[] newdata)
        {
            List<nodeinfo> tmpnodes = nodes;
            List<String> deletenodes = new List<string>();

            tmpnodes.Sort((x, y) => x.name.CompareTo(y.name));
            Array.Sort(newdata);

            for (int i = 0; i < tmpnodes.Count; i++)
            {
                Console.WriteLine("{" + tmpnodes[i] + "}");
                bool isLonely = true;
                for (int j = i; j < newdata.Length; j++)
                {
                    Console.WriteLine(newdata[j]);
                    if (tmpnodes[i].name == newdata[j])
                    {
                        isLonely = false;
                        break;
                    }
                }
                if (isLonely)
                {
                    deletenodes.Add(tmpnodes[i].name);
                    tmpnodes.RemoveAt(i);
                    i--;
                }
            }
            // Delete nodes in lists of edges
            tmpnodes = DeleteRemains(deletenodes, tmpnodes);
            return tmpnodes;
        }
        protected List<nodeinfo> DeleteRemains(List <string> deletenodes,  List <nodeinfo> targetnodes)
        {
            for(int i =0; i<targetnodes.Count; i++)
            {
                for(int j = 0; j < targetnodes[i].neighbors.Count; j++)
                    foreach(var n in deletenodes)
                    {
                        if (targetnodes[i].neighbors[j].endname == n)
                            targetnodes[i].neighbors.RemoveAt(j);
                    }
            }
            return targetnodes;

        }

    }
}
