output "cluster_name" {
  value = aws_eks_cluster.fastfood_cluster.name
}

output "node_group_name" {
  value = aws_eks_node_group.fastfood_node_group.node_group_name
}