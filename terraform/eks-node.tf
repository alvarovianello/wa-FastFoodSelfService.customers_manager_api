resource "aws_eks_node_group" "eks-node-customer"{
    cluster_name = aws_eks_cluster.eks-cluster-customer.name
    node_group_name = var.nodeGrop
    node_role_arn = var.labRole
    subnet_ids = [for subnet in data.aws_subnet.subnet : subnet.id if subnet.availability_zone != "${var.regionDefault}e" ]

    scaling_config {
        desired_size = 2
        max_size     = 3
        min_size     = 1
    }

    update_config {
        max_unavailable = 1
    }
}
